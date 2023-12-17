namespace Flare.Extensions;

public static class ValueExtensions
{
    /// <summary>
    /// Returns true if all the values in the specified list is not equal to null, empty, or whitespace, otherwise, false.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool IsEmpty(this IList<string> values)
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(values[i]))
                continue;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Returns true if at least one value in the specified list is not equal to null, empty, or whitespace, otherwise, false.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool IsNotEmpty(this IList<string> values) => !IsEmpty(values);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IReadOnlyList<T> GetDataOrEmpty<T>(this List<T>? data) => data ?? new List<T>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetDataOrDefault<T>(this T? data) => data is null ? default : data;
}