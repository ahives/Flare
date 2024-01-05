namespace Flare.API.Internal;

using Extensions;
using Model;

public partial class AlertImpl
{
    public async Task<Maybe<AlertAllInfo>> GetAll(Action<QueryAllAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new QueryAllAlertCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;
        string baseUrl = "alerts";
        var errors = qc.Validate();
        if (errors.Any())
            return Response.Failed<AlertAllInfo>(Debug.WithErrors(baseUrl, errors));

        string url = qc.IsSearchQuery()
            ? $"{baseUrl}?query={qc.GetQueryArguments().BuildQueryString()}"
            : $"{baseUrl}?{qc.GetQueryArguments().BuildQueryString()}";

        return await GetRequest<AlertAllInfo>(url, cancellationToken);
    }

    
    class QueryAllAlertCriteriaImpl :
        QueryAllAlertCriteria, IQueryCriteria
    {
        IdentifierType _identifierType;
        string _identifier;
        IQueryCriteria _criteria;
        int _offset;
        int _limit;
        SortableFields _sortField;
        OrderType _orderType;

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

        public bool IsSearchQuery() => _criteria != null && _criteria.IsSearchQuery();

        public IReadOnlyList<Error> Validate()
        {
            var errors = new List<Error>();

            if (_offset < 0)
                errors.Add(Errors.Create(ErrorType.PaginationOffset, $"Pagination offset must be greater than or equal to 0."));

            if (IsSearchQuery())
            {
                errors.AddRange(_criteria.Validate());
            }
            else
            {
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
                        "Identifier type is not compatible with identifier."));

                if (isGuid && _identifierType != IdentifierType.Id)
                    errors.Add(Errors.Create(ErrorType.IdentifierType,
                        "Identifier type is not compatible with identifier."));

                if (isGuid && identifier != default && isIdentifierTypeMissing ||
                    (string.IsNullOrWhiteSpace(_identifier) && isIdentifierTypeMissing))
                    errors.Add(Errors.Create(ErrorType.IdentifierType, "Identifier type is missing."));
            }

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

            string orderType = _orderType switch
            {
                OrderType.Desc => "desc",
                OrderType.Asc => "asc",
                _ => string.Empty
            };

            if (IsSearchQuery())
            {
                arguments = _criteria.GetQueryArguments();
            }
            else
            {
                arguments.Add("searchIdentifier", new QueryArg{Value = _identifier});
                arguments.Add("searchIdentifierType", new QueryArg{Value = searchIdentifierType});
            }

            arguments.Add("offset", new QueryArg{Value = _offset});
            arguments.Add("limit", new QueryArg{Value = _limit});
            arguments.Add("sort", new QueryArg{Value = sortField});
            arguments.Add("order", new QueryArg{Value = orderType});

            return arguments;
        }


        class SearchQueryCriteriaImpl :
            SearchQueryCriteria, IQueryCriteria
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