using Flare.Model;

namespace Flare.Internal;

public partial class AlertImpl
{
    public async Task<Result<AlertData>> Get(Guid identifier, CancellationToken cancellationToken = default)
    {
        return new SuccessfulResult<AlertData> {DebugInfo = new DebugInfo()};
    }
}