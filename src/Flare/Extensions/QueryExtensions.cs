namespace Flare.Extensions;

using System.Text;

public static class QueryExtensions
{
    public static string BuildQueryString(this IDictionary<string, QueryArg> arguments)
    {
        StringBuilder builder = new StringBuilder();
        var keys = arguments.Keys.ToList();
        
        for (int i = 0; i < arguments.Keys.Count; i++)
        {
            string key = keys[i];
            string arg = arguments[key].IsSearchQuery ? $"{key}:{arguments[key].Value}" : $"{key}={arguments[key].Value}";

            if (i == 0)
            {
                builder.AppendFormat(arg);
                continue;
            }
            
            builder.AppendFormat($"&{arg}");
        }

        return builder.ToString();
    }
}