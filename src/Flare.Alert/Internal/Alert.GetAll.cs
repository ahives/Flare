namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<AlertAllInfo>> GetAll(Action<QueryAllAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new GetAllAlertsImpl();
        criteria?.Invoke(impl);

        var errors = impl.Validate();
        if (errors.Any())
            return Response.Failed<AlertAllInfo>(Debug.WithErrors("alerts", errors));

        string url = $"alerts{impl.GetQueryArguments().BuildQueryString()}";

        return await GetRequest<AlertAllInfo>(url, Serializer.Options, cancellationToken);
    }

    
    class GetAllAlertsImpl :
        QueryAllAlertCriteria,
        IQueryCriteria,
        IValidator
    {
        IdentifierType _identifierType;
        string _identifier;
        int _offset;
        int _limit;
        SortableFields _sortField;
        OrderType _orderType;
        AlertStatus? _status;

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

        public void PaginationOffset(int offset)
        {
            _offset = offset;
        }

        public void PaginationLimit(int limit)
        {
            _limit = limit;
        }

        public void SortBy(SortableFields field)
        {
            _sortField = field;
        }

        public void OrderBy(OrderType type)
        {
            _orderType = type;
        }

        public IReadOnlyList<Error> Validate()
        {
            var errors = new List<Error>();

            if (_offset < 0)
                errors.Add(Errors.Create(ErrorType.PaginationOffset, "Pagination offset must be greater than or equal to 0."));

            string sortField = _sortField switch
            {
                SortableFields.CreatedAt => "createdAt",
                SortableFields.UpdatedAt => "updatedAt",
                SortableFields.TinyId => "tinyId",
                SortableFields.Alias => "alias",
                SortableFields.Message => "message",
                SortableFields.Status => "status",
                SortableFields.Acknowledged => "acknowledged",
                SortableFields.IsSeen => "isSeen",
                SortableFields.Snoozed => "snoozed",
                SortableFields.SnoozedUntil => "snoozedUntil",
                SortableFields.Count => "count",
                SortableFields.LastOccurredAt => "lastOccurredAt",
                SortableFields.Source => "source",
                SortableFields.Owner => "owner",
                SortableFields.ReportClosedBy => "report.closedBy",
                SortableFields.ReportAcknowledgedBy => "report.acknowledgedBy",
                SortableFields.ReportCloseTime => "report.closeTime",
                SortableFields.ReportAckTime => "report.ackTime",
                SortableFields.IntegrationName => "integration.name",
                SortableFields.IntegrationType => "integration.type",
                _ => string.Empty
            };

            if (string.IsNullOrWhiteSpace(sortField))
                errors.Add(Errors.Create(ErrorType.SortField, "Sortable field is not valid in the current context."));

            if (_status.HasValue)
            {
                bool isStatusMissing = _status switch
                {
                    AlertStatus.Open => false,
                    AlertStatus.Closed => false,
                    _ => true
                };

                if (isStatusMissing)
                    errors.Add(Errors.Create(ErrorType.AlertStatusIncompatible,
                        $"{_status.ToString()} is not a valid alert status in the current context."));

                return errors;
            }

            errors.AddRange(
                _identifier.ValidateIdType(_identifierType, t => t switch
                {
                    IdentifierType.Id => false,
                    IdentifierType.Name => false,
                    _ => true
                }));

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments()
        {
            var arguments = new Dictionary<string, QueryArg>();
            string searchIdentifierType = _identifierType switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Name => "name",
                _ => string.Empty
            };

            string sortField = _sortField switch
            {
                SortableFields.CreatedAt => "createdAt",
                SortableFields.UpdatedAt => "updatedAt",
                SortableFields.TinyId => "tinyId",
                SortableFields.Alias => "alias",
                SortableFields.Message => "message",
                SortableFields.Status => "status",
                SortableFields.Acknowledged => "acknowledged",
                SortableFields.IsSeen => "isSeen",
                SortableFields.Snoozed => "snoozed",
                SortableFields.SnoozedUntil => "snoozedUntil",
                SortableFields.Count => "count",
                SortableFields.LastOccurredAt => "lastOccurredAt",
                SortableFields.Source => "source",
                SortableFields.Owner => "owner",
                SortableFields.ReportClosedBy => "report.closedBy",
                SortableFields.ReportAcknowledgedBy => "report.acknowledgedBy",
                SortableFields.ReportCloseTime => "report.closeTime",
                SortableFields.ReportAckTime => "report.ackTime",
                SortableFields.IntegrationType => "integration.type",
                SortableFields.IntegrationName => "integration.name",
                _ => string.Empty
            };

            if (!string.IsNullOrWhiteSpace(sortField))
                arguments.Add("sort", new QueryArg {Value = sortField});

            string orderType = _orderType switch
            {
                OrderType.Desc => "desc",
                OrderType.Asc => "asc",
                _ => string.Empty
            };

            if (_status.HasValue)
            {
                string status = _status switch
                {
                    AlertStatus.Open => "open",
                    AlertStatus.Closed => "close",
                    _ => string.Empty
                };

                var args = new Dictionary<string, QueryArg>();
                if (!string.IsNullOrWhiteSpace(status))
                    args.Add("status", new QueryArg {IsQuery = true, Value = status});

                args.Add("offset", new QueryArg{Value = _offset});
                args.Add("limit", new QueryArg{Value = _limit});

                if (!string.IsNullOrWhiteSpace(orderType))
                    args.Add("order", new QueryArg{Value = orderType});

                if (!string.IsNullOrWhiteSpace(sortField))
                    args.Add("sort", new QueryArg{Value = sortField});

                return args;
            }

            arguments.Add("offset", new QueryArg{Value = _offset});
            arguments.Add("limit", new QueryArg{Value = _limit});

            if (!string.IsNullOrWhiteSpace(orderType))
                arguments.Add("order", new QueryArg{Value = orderType});

            if (!string.IsNullOrWhiteSpace(_identifier))
                arguments.Add("searchIdentifier", new QueryArg {Value = _identifier});

            if (!string.IsNullOrWhiteSpace(searchIdentifierType))
                arguments.Add("searchIdentifierType", new QueryArg {Value = searchIdentifierType});

            return arguments;
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