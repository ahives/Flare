namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result> Delete(Guid identifier, Action<AlertDeleteQuery> query, CancellationToken cancellationToken = default)
    {
        var impl = new AlertDeleteQueryImpl();
        query?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}?{queryString}";
        
        return new SuccessfulResult {DebugInfo = new DebugInfo{URL = url}};
    }

    
    class AlertDeleteQueryImpl :
        AlertDeleteQuery
    {
        public IDictionary<string, object> QueryArguments { get; }

        public AlertDeleteQueryImpl()
        {
            QueryArguments = new Dictionary<string, object>();
        }

        public void SearchIdentifierType(DeleteSearchIdentifierType type)
        {
            string searchIdentifierType = type switch
            {
                DeleteSearchIdentifierType.AlertId => "AlertID",
                DeleteSearchIdentifierType.TinyId => "tinyID",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            QueryArguments.Add("identifierType", searchIdentifierType);
        }

        public void User(string displayName)
        {
            QueryArguments.Add("user", displayName);
        }

        public void Source(string displayName)
        {
            QueryArguments.Add("source", displayName);
        }
    }
}