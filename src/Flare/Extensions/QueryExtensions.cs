namespace Flare.Extensions;

using System.Text;

internal static class QueryExtensions
{
    public static string BuildQueryString(this IDictionary<string, object> arguments)
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

    public static string BuildSubQueryString(this IDictionary<string, object> arguments)
    {
        StringBuilder builder = new StringBuilder();
        var keys = arguments.Keys.ToList();
        
        for (int i = 0; i < arguments.Keys.Count; i++)
        {
            string key = keys[i];

            if (i == 0)
            {
                builder.AppendFormat($"{key}:{arguments[key]}");
                continue;
            }
            
            builder.AppendFormat($"&{key}:{arguments[key]}");
        }

        return builder.ToString();
    }
}