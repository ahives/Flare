namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Result<ListAlertResponse>> List(Action<ListAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        var impl = new ListAlertCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? "https://api.opsgenie.com/v2/alerts"
            : $"https://api.opsgenie.com/v2/alerts?{queryString}";

        return await GetRequest<ListAlertResponse>(url, cancellationToken);
    }

    
    class ListAlertCriteriaImpl :
        ListAlertCriteria
    {
        public IDictionary<string, object> QueryArguments { get; }

        public ListAlertCriteriaImpl()
        {
            QueryArguments = new Dictionary<string, object>();
        }

        public void SearchIdentifier(Guid searchIdentifier)
        {
            QueryArguments.Add("searchIdentifier", searchIdentifier);
        }

        public void SearchIdentifierType(QuerySearchIdentifierType type)
        {
            string searchIdentifierType = type switch
            {
                QuerySearchIdentifierType.Id => "id",
                QuerySearchIdentifierType.Name => "name",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
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
                _ => throw new ArgumentOutOfRangeException(nameof(field), field, null)
            };
            
            QueryArguments.Add("sort", sortField);
        }

        public void Order(OrderType type)
        {
            string orderType = type switch
            {
                OrderType.Desc => "desc",
                OrderType.Asc => "asc",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            QueryArguments.Add("order", orderType);
        }
    }
}