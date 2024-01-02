namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Maybe<AlertStatusData>> GetStatus(Guid requestId, CancellationToken cancellationToken = default)
    {
        string url = $"https://api.opsgenie.com/v2/alerts/requests/{requestId}";

        return await GetRequest<AlertStatusData>(url, cancellationToken);
    }
}