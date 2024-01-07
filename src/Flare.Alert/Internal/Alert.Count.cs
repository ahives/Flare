namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<AlertCountInfo>> Count(Action<CountAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new CountAlertQueryCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;
        string baseUrl = "alerts/count";
        var errors = qc.Validate();
        if (errors.Count != 0)
            return Response.Failed<AlertCountInfo>(Debug.WithErrors(baseUrl, errors));

        string url = qc.IsSearchQuery()
            ? $"{baseUrl}?query={QueryExtensions.BuildQueryString(qc.GetQueryArguments())}"
            : $"{baseUrl}?{QueryExtensions.BuildQueryString(qc.GetQueryArguments())}";

        return await GetRequest<AlertCountInfo>(url, Serializer.Options, cancellationToken);
    }

    
    class CountAlertQueryCriteriaImpl :
        CountAlertCriteria,
        IQueryCriteria
    {
        IdentifierType _identifierType;
        string _identifier;
        IQueryCriteria _criteria;

        public void Query(Action<SearchQueryCriteria> criteria)
        {
            var impl = new SearchQueryCriteriaImpl();
            criteria?.Invoke(impl);

            _criteria = impl;
        }

        public void SearchIdentifier(string identifier)
        {
            _identifier = identifier;
        }

        public void SearchIdentifierType(IdentifierType type)
        {
            _identifierType = type;
        }

        public bool IsSearchQuery() => _criteria != null && _criteria.IsSearchQuery();

        public IReadOnlyList<Error> Validate()
        {
            if (IsSearchQuery())
                return _criteria.Validate();

            var errors = new List<Error>();
            bool isIdentifierTypeMissing = _identifierType switch
            {
                IdentifierType.Id => false,
                IdentifierType.Name => false,
                _ => true
            };

            if (isIdentifierTypeMissing)
                errors.Add(Errors.Create(ErrorType.IdentifierType,
                    $"{_identifierType.ToString()} is not a valid identifier type in the current context."));

            bool isGuid = Guid.TryParse(_identifier, out var identifier);

            if (!isGuid && _identifierType != IdentifierType.Name)
                errors.Add(Errors.Create(ErrorType.IdentifierType,
                    $"Identifier type is not compatible with identifier."));

            if (isGuid && identifier != default && isIdentifierTypeMissing ||
                (string.IsNullOrWhiteSpace(_identifier) && isIdentifierTypeMissing))
                errors.Add(Errors.Create(ErrorType.IdentifierType, $"Identifier type is missing."));

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments()
        {
            if (IsSearchQuery())
                return _criteria.GetQueryArguments();

            var arguments = new Dictionary<string, QueryArg>();
            string searchIdentifierType = _identifierType switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Name => "name",
                _ => string.Empty
            };
            
            arguments.Add("searchIdentifier", new QueryArg{Value = _identifier});
            arguments.Add("searchIdentifierType", new QueryArg{Value = searchIdentifierType});

            return arguments;
        }


        class SearchQueryCriteriaImpl :
            SearchQueryCriteria,
            IQueryCriteria
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

            public Dictionary<string, QueryArg> GetQueryArguments()
            {
                var arguments = new Dictionary<string, QueryArg>();
                string status = _status switch
                {
                    AlertStatus.Open => "open",
                    AlertStatus.Closed => "close",
                    _ => string.Empty
                };

                arguments.Add("status", new QueryArg{IsSearchQuery = true, Value = status});

                return arguments;
            }

            public bool IsSearchQuery() => true;
        }
    }
}