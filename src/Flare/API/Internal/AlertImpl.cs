namespace Flare.API.Internal;

using System.Text;

public partial class AlertImpl :
    FlareHttpClient,
    Alert
{
    public AlertImpl(HttpClient client) : base(client)
    {
    }

    string BuildQueryString(IDictionary<string, object> arguments)
    {
        StringBuilder builder = new StringBuilder();
        var keys = arguments.Keys.ToList();
        
        for (int i = 0; i < arguments.Keys.Count; i++)
        {
            string key = keys[i];

            if (i == 0)
            {
                builder.AppendFormat($"{key}={arguments[key]}");
                continue;
            }
            
            builder.AppendFormat($"&{key}={arguments[key]}");
        }

        return builder.ToString();
    }
}