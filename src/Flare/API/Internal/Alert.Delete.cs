namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Result<AlertResponse>> Delete(Guid identifier, Action<DeleteAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        var impl = new DeleteAlertCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}?{queryString}";

        return await DeleteRequest<AlertResponse>(url, cancellationToken).ConfigureAwait(false);
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
            string identifierType = type switch
            {
                DeleteSearchIdentifierType.AlertId => "AlertID",
                DeleteSearchIdentifierType.TinyId => "tinyID",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            QueryArguments.Add("identifierType", identifierType);
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