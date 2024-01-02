namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<AlertInfo>> Get(string identifier, IdentifierType identifierType, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IDictionary<string, object> QueryIdentifierType(IdentifierType type)
        {
            var args = new Dictionary<string, object>();
            string searchIdentifierType = type switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Alias => "alias",
                IdentifierType.Tiny => "tiny",
                _ => String.Empty
            };
            
            args.Add("identifierType", searchIdentifierType);

            return args;
        }

        var args = QueryIdentifierType(identifierType);
        string queryString = BuildQueryString(args);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}?{queryString}";

        return await GetRequest<AlertInfo>(url, cancellationToken);
    }
}