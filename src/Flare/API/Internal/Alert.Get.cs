using Flare.API.Model;

namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result<AlertData>> Get(Guid identifier, Action<AlertGetCriteria> criteria, CancellationToken cancellationToken = default)
    {
        var impl = new AlertGetCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}?{queryString}";
        
        return new SuccessfulResult<AlertData> {DebugInfo = new DebugInfo{URL = url}};
    }

    
    class AlertGetCriteriaImpl :
        AlertGetCriteria
    {
        public IDictionary<string, object> QueryArguments { get; }

        public AlertGetCriteriaImpl()
        {
            QueryArguments = new Dictionary<string, object>();
        }

        public void SearchIdentifierType(GetQuerySearchIdentifierType type)
        {
            string searchIdentifierType = type switch
            {
                GetQuerySearchIdentifierType.Id => "id",
                GetQuerySearchIdentifierType.Alias => "alias",
                GetQuerySearchIdentifierType.Tiny => "tiny",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            QueryArguments.Add("identifierType", searchIdentifierType);
        }
    }
}