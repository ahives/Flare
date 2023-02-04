namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result<AlertStatusData>> GetStatus(Guid requestId, CancellationToken cancellationToken = default)
    {
        return new SuccessfulResult<AlertStatusData> {DebugInfo = new DebugInfo() };
    }
}