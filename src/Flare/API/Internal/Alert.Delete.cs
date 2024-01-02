namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<AlertResponse>> Delete(Guid identifier, IdentifierType identifierType, Action<DeleteAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new DeleteAlertCriteriaImpl();
        criteria?.Invoke(impl);

        impl.QueryArguments.Add("identifierType", GetIdentifierType(identifierType));

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}?{queryString}";

        return await DeleteRequest<AlertResponse>(url, cancellationToken).ConfigureAwait(false);

        string GetIdentifierType(IdentifierType type) =>
            type switch
            {
                IdentifierType.AlertId => "AlertID",
                IdentifierType.TinyId => "tinyID",
                _ => string.Empty
            };
    }

    
    class DeleteAlertCriteriaImpl :
        DeleteAlertCriteria
    {
        public IDictionary<string, object> QueryArguments { get; }

        public DeleteAlertCriteriaImpl()
        {
            QueryArguments = new Dictionary<string, object>();
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