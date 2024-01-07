namespace Flare.Alert.Internal;

using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<AlertStatusInfo>> Status(Guid requestId, CancellationToken cancellationToken = default)
    {
        string url = $"alerts/requests/{requestId}";

        return await GetRequest<AlertStatusInfo>(url, Serializer.Options, cancellationToken);
    }
}