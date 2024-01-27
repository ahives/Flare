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

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<AlertCountInfo>(Debug.WithErrors("alerts/count", errors));

        string url = $"alerts/count{impl.GetQueryArguments().BuildQueryString()}";

        return await GetRequest<AlertCountInfo>(url, Serializer.Options, cancellationToken);
    }

    
    class CountAlertQueryCriteriaImpl :
        CountAlertCriteria,
        IQueryCriteria,
        IValidator
    {
        IdentifierType? _identifierType;
        string _identifier;
        private AlertStatus? _status;

        public CountAlertQueryCriteriaImpl()
        {
            _identifierType = null;
        }

        public void Query(Action<SearchQueryCriteria> criteria)
        {
            var impl = new SearchQueryCriteriaImpl();
            criteria?.Invoke(impl);

            _status = impl.AlertStatus;
        }

        public void SearchIdentifier(string identifier)
        {
            _identifier = identifier;
        }

        public void SearchIdentifierType(IdentifierType type)
        {
            _identifierType = type;
        }

        public IReadOnlyList<Error> Validate()
        {
            if (!_status.HasValue)
                return _identifier.ValidateNullableIdType(_identifierType, t => t switch
                {
                    IdentifierType.Id => false,
                    IdentifierType.Name => false,
                    _ => true
                });

            bool isStatusMissing = _status switch
            {
                AlertStatus.Open => false,
                AlertStatus.Closed => false,
                _ => true
            };

            var errors = new List<Error>();
            if (isStatusMissing)
                errors.Add(Errors.Create(ErrorType.AlertStatusIncompatible,
                    $"{_status.ToString()} is not a valid alert status in the current context."));

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments()
        {
            if (_status.HasValue)
            {
                string status = _status switch
                {
                    AlertStatus.Open => "open",
                    AlertStatus.Closed => "close",
                    _ => string.Empty
                };

                return new Dictionary<string, QueryArg> {{"status", new QueryArg{IsSearchQuery = true, Value = status}}};
            }

            string identifierType = _identifierType switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Name => "name",
                _ => string.Empty
            };

            return !string.IsNullOrWhiteSpace(_identifier) && !string.IsNullOrWhiteSpace(identifierType)
                ? new Dictionary<string, QueryArg>
                {
                    {"searchIdentifier", new QueryArg {Value = _identifier}},
                    {"searchIdentifierType", new QueryArg {Value = identifierType}}
                }
                : new Dictionary<string, QueryArg>();
        }


        class SearchQueryCriteriaImpl :
            SearchQueryCriteria
        {
            public AlertStatus? AlertStatus { get; private set; }

            public void Status(AlertStatus status)
            {
                AlertStatus = status;
            }
        }
    }
}