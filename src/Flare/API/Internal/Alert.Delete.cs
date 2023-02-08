namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result> Delete(Guid identifier, Action<DeleteAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        var impl = new DeleteAlertCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}?{queryString}";
        
        return new SuccessfulResult {DebugInfo = new DebugInfo{URL = url}};
    }

    
    class DeleteAlertCriteriaImpl :
        DeleteAlertCriteria
    {
        public IDictionary<string, object> QueryArguments { get; }

        public DeleteAlertCriteriaImpl()
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