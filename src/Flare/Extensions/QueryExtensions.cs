namespace Flare.Extensions;

using System.Text;
using Model;

public static class QueryExtensions
{
    public static string BuildQueryString(this IDictionary<string, QueryArg> arguments)
    {
        StringBuilder builder = new StringBuilder();
        var keys = arguments.Keys.ToList();
        bool isQuery = false;
        
        for (int i = 0; i < keys.Count; i++)
        {
            string key = keys[i];
            string arg = arguments[key].IsQuery ? $"{key}%3A{arguments[key].Value}" : $"{key}={arguments[key].Value}";

            if (!isQuery)
                isQuery = arguments[key].IsQuery;
            
            if (i == 0)
            {
                builder.AppendFormat(arg);
                continue;
            }
            
            builder.AppendFormat($"&{arg}");
        }

        string queryDelimiter = isQuery ? "?query=" : "?";

        return keys.Count > 0 ? $"{queryDelimiter}{builder}" : string.Empty;
    }
}