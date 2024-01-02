namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<AlertCountInfo>> Count(Action<CountAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new CountAlertCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? "https://api.opsgenie.com/v2/alerts/count"
            : $"https://api.opsgenie.com/v2/alerts/count?{queryString}";

        if (impl.Errors.Any())
            return Response.Failed<AlertCountInfo>(Debug.WithErrors(url, impl.Errors));

        return await GetRequest<AlertCountInfo>(url, cancellationToken);
    }

    
    class CountAlertCriteriaImpl :
        CountAlertCriteria
    {
        public IDictionary<string, object> QueryArguments { get; }
        public List<Error> Errors = new();

        public CountAlertCriteriaImpl()
        {
            QueryArguments = new Dictionary<string, object>();
        }

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
    }
}