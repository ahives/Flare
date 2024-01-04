namespace Flare.API.Internal;

using Extensions;
using Model;

public partial class AlertImpl
{
    public async Task<Maybe<AlertCountInfo>> Count(Action<CountAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new CountAlertQueryCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;
        string baseUrl = "https://api.opsgenie.com/v2/alerts/count";
        var errors = qc.Validate();
        if (errors.Count != 0)
            return Response.Failed<AlertCountInfo>(Debug.WithErrors(baseUrl, errors));

        string url = qc.IsQuery()
            ? $"{baseUrl}?query={qc.GetQueryArguments().BuildSubQueryString()}"
            : $"{baseUrl}?{qc.GetQueryArguments().BuildQueryString()}";

        //https://api.opsgenie.com/v2/alerts/count?searchIdentifier=70413a06-38d6-4c85-92b8-5ebc900d42e2&searchIdentifierType=id
        //https://api.opsgenie.com/v2/alerts/count?query=status%3Aopen
        //https://api.opsgenie.com/v2/alerts/count?searchIdentifier=openAlerts&searchIdentifierType=name
        return await GetRequest<AlertCountInfo>(url, cancellationToken);
    }

    
    class CountAlertQueryCriteriaImpl :
        CountAlertCriteria
    {
        IdentifierType _identifierType;
        string _identifier;
        IQueryCriteria _qc;

        public void Query(Action<SearchQueryCriteria> criteria)
        {
            var impl = new SearchQueryCriteriaImpl();
            criteria?.Invoke(impl);

            _qc = impl;
        }

        public void SearchIdentifier(string identifier)
        {
            _identifier = identifier;
        }

        public void SearchIdentifierType(IdentifierType type)
        {
            _identifierType = type;
        }

        public bool IsQuery() => _qc != null && _qc.IsQuery();

        public IReadOnlyList<Error> Validate()
        {
            if (IsQuery())
            {
                var queryErrors = _qc.Validate();
                if (queryErrors.Count != 0)
                    return queryErrors;
            }

            var errors = new List<Error>();
            bool isIdentifierTypeMissing = _identifierType switch
            {
                IdentifierType.Id => false,
                IdentifierType.Name => false,
                _ => true
            };

            if (isIdentifierTypeMissing)
                errors.Add(Errors.Create(ErrorType.IdentifierType, $"{_identifierType.ToString()} is not a valid identifier type in the current context."));

            bool isGuid = Guid.TryParse(_identifier, out var identifier);

            if (!isGuid && _identifierType != IdentifierType.Name)
                errors.Add(Errors.Create(ErrorType.IdentifierType, $"Identifier type is not compatible with identifier."));

            if (isGuid && identifier != default && isIdentifierTypeMissing || (string.IsNullOrWhiteSpace(_identifier) && isIdentifierTypeMissing))
                errors.Add(Errors.Create(ErrorType.IdentifierType, $"Identifier type is missing."));

            return errors;
        }

        public IDictionary<string, object> GetQueryArguments()
        {
            if (IsQuery())
            {
                var args = _qc.GetQueryArguments();
                if (args.Count != 0)
                    return args;
            }

            var arguments = new Dictionary<string, object>();
            string searchIdentifierType = _identifierType switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Name => "name",
                _ => string.Empty
            };
            
            arguments.Add("searchIdentifier", _identifier);
            arguments.Add("searchIdentifierType", searchIdentifierType);

            return arguments;
        }


        class SearchQueryCriteriaImpl :
            SearchQueryCriteria
        {
            AlertStatus _status;

            public void Status(AlertStatus status)
            {
                _status = status;
            }

            public IReadOnlyList<Error> Validate()
            {
                var errors = new List<Error>();
                bool isStatusMissing = _status switch
                {
                    AlertStatus.Open => false,
                    AlertStatus.Closed => false,
                    _ => true
                };

                if (isStatusMissing)
                    errors.Add(Errors.Create(ErrorType.AlertStatus, $"{_status.ToString()} is not a valid alert status in the current context."));

                return errors;
            }

            public IDictionary<string, object> GetQueryArguments()
            {
                var arguments = new Dictionary<string, object>();

                arguments.Add("status", _status switch
                {
                    AlertStatus.Open => "open",
                    AlertStatus.Closed => "close",
                    _ => string.Empty
                });

                return arguments;
            }

            public bool IsQuery() => true;
        }
    }
}