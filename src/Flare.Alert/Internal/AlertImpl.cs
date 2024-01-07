namespace Flare.Alert.Internal;

public partial class AlertImpl :
    FlareHttpClient,
    Alert
{
    public AlertImpl(HttpClient client) : base(client)
    {
    }
}