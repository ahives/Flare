using Flare.API.Model;

namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result<AlertCountData>> Count(Action<AlertCountQuery> query, CancellationToken cancellationToken = default)
    {
        var impl = new AlertCountQueryImpl();
        query?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? "https://api.opsgenie.com/v2/alerts/count"
            : $"https://api.opsgenie.com/v2/alerts/count?{queryString}";
        
        return new SuccessfulResult<AlertCountData> {DebugInfo = new DebugInfo{URL = url}};
    }

    
    class AlertCountQueryImpl :
        AlertCountQuery
    {
        public IDictionary<string, object> QueryArguments { get; }

        public AlertCountQueryImpl()
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
    }
}