using System.Text;
using Flare.Model;

namespace Flare.Internal;

public partial class AlertImpl
{
    public async Task<Result<AbbreviatedAlertData>> List(Action<AlertListQuery> query, CancellationToken cancellationToken = default)
    {
        var impl = new AlertListQueryImpl();
        query?.Invoke(impl);

        string queryString = BuildQueryString(impl.Query);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? "https://api.opsgenie.com/v2/alerts"
            : $"https://api.opsgenie.com/v2/alerts?{queryString}";
        
        return new SuccessfulResult<AbbreviatedAlertData> {DebugInfo = new DebugInfo{URL = url}};
    }

    string BuildQueryString(IDictionary<string, object> map)
    {
        StringBuilder builder = new StringBuilder();
        var keys = map.Keys.ToList();
        
        for (int i = 0; i < map.Keys.Count; i++)
        {
            string key = keys[i];

            if (i == 0)
            {
                builder.AppendFormat($"{key}={map[key]}");
                continue;
            }
            
            builder.AppendFormat($"&{key}={map[key]}");
        }

        return builder.ToString();
    }

    
    class AlertListQueryImpl :
        AlertListQuery
    {
        public IDictionary<string, object> Query { get; }

        public AlertListQueryImpl()
        {
            Query = new Dictionary<string, object>();
        }

        public void SearchIdentifier(Guid searchIdentifier)
        {
            Query.Add("searchIdentifier", searchIdentifier);
        }

        public void SearchIdentifierType(QuerySearchIdentifierType type)
        {
            string searchIdentifierType = type switch
            {
                QuerySearchIdentifierType.Id => "id",
                QuerySearchIdentifierType.Name => "name",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            Query.Add("searchIdentifierType", searchIdentifierType);
        }

        public void Offset(int offset)
        {
            Query.Add("offset", offset);
        }

        public void Limit(int limit)
        {
            Query.Add("limit", limit);
        }

        public void Sort(SortField field)
        {
            string sortField = field switch
            {
                SortField.CreatedAt => "createdAt",
                SortField.UpdatedAt => "updatedAt",
                SortField.TinyId => "tinyId",
                SortField.Alias => "alias",
                SortField.Message => "message",
                SortField.Status => "status",
                SortField.Acknowledged => "acknowledged",
                SortField.IsSeen => "isSeen",
                SortField.Snoozed => "snoozed",
                SortField.SnoozedUntil => "snoozedUntil",
                SortField.Count => "count",
                SortField.LastOccurredAt => "lastOccurredAt",
                SortField.Source => "source",
                SortField.Owner => "owner",
                SortField.ReportClosedBy => "report.closedBy",
                SortField.ReportAcknowledgedBy => "report.acknowledgedBy",
                SortField.ReportCloseTime => "report.closeTime",
                SortField.ReportAckTime => "report.ackTime",
                SortField.IntegrationType => "integration.type",
                SortField.IntegrationName => "integration.name",
                _ => throw new ArgumentOutOfRangeException(nameof(field), field, null)
            };
            
            Query.Add("sort", sortField);
        }

        public void Order(OrderType type)
        {
            string orderType = type switch
            {
                OrderType.Desc => "desc",
                OrderType.Asc => "asc",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            Query.Add("order", orderType);
        }
    }
}