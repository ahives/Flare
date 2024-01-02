namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Maybe<AlertStatusInfo>> Status(Guid requestId, CancellationToken cancellationToken = default)
    {
        string url = $"https://api.opsgenie.com/v2/alerts/requests/{requestId}";

        return await GetRequest<AlertStatusInfo>(url, cancellationToken);
    }
}