namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<AlertAllInfo>> GetAll(Action<QueryAllAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new QueryAllAlertCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? "https://api.opsgenie.com/v2/alerts"
            : $"https://api.opsgenie.com/v2/alerts?{queryString}";

        if (impl.Errors.Any())
            return Response.Failed<AlertAllInfo>(Debug.WithErrors(url, impl.Errors));

        return await GetRequest<AlertAllInfo>(url, cancellationToken);
    }

    
    class QueryAllAlertCriteriaImpl :
        QueryAllAlertCriteria
    {
        public IDictionary<string, object> QueryArguments { get; } = new Dictionary<string, object>();
        public List<Error> Errors = new();

        public void SearchIdentifier(Guid identifier)
        {
            QueryArguments.Add("searchIdentifier", identifier);
        }

        public void SearchIdentifierType(IdentifierType type)
        {
            string searchIdentifierType = type switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Name => "name",
                _ => string.Empty
            };

            if (string.IsNullOrWhiteSpace(searchIdentifierType))
                Errors.Add(new Error{Reason = $"{type.ToString()} is not valid in the current context.", Timestamp = DateTimeOffset.UtcNow});

            QueryArguments.Add("searchIdentifierType", searchIdentifierType);
        }

        public void Offset(int offset)
        {
            QueryArguments.Add("offset", offset);
        }

        public void Limit(int limit)
        {
            QueryArguments.Add("limit", limit);
        }

        public void Sort(SortableFields field)
        {
            string sortField = field switch
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

            if (string.IsNullOrWhiteSpace(sortField))
                Errors.Add(new Error{Reason = "is not valid in the current context.", Timestamp = DateTimeOffset.UtcNow});

            QueryArguments.Add("sort", sortField);
        }

        public void Order(OrderType type)
        {
            string orderType = type switch
            {
                OrderType.Desc => "desc",
                OrderType.Asc => "asc",
                _ => string.Empty
            };

            if (string.IsNullOrWhiteSpace(orderType))
                Errors.Add(new Error{Reason = "is not valid in the current context.", Timestamp = DateTimeOffset.UtcNow});

            QueryArguments.Add("order", orderType);
        }
    }
}