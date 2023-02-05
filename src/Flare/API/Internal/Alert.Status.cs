namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result<AlertStatusData>> GetStatus(Guid requestId, CancellationToken cancellationToken = default)
    {
        string url = $"https://api.opsgenie.com/v2/alerts/requests/{requestId}";
        
        return new SuccessfulResult<AlertStatusData> {DebugInfo = new DebugInfo{URL = url} };
    }
}