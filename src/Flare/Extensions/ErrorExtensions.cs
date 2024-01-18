namespace Flare.Extensions;

using Model;

public static class ErrorExtensions
{
    public static IEnumerable<Error> Concat(this IReadOnlyList<Error> value, IReadOnlyList<Error> errors)
    {
        for (int i = 0; i < value.Count; i++)
            yield return value[i];

        for (int i = 0; i < errors.Count; i++)
            yield return errors[i];
    }
}