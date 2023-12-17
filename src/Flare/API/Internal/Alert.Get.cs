using Flare.API.Model;

namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result<GetAlertResponse>> Get(string identifier, Action<GetAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        var impl = new GetAlertCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}?{queryString}";

        return await GetRequest<GetAlertResponse>(url, cancellationToken);
    }

    
    class GetAlertCriteriaImpl :
        GetAlertCriteria
    {
        public IDictionary<string, object> QueryArguments { get; }

        public GetAlertCriteriaImpl()
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