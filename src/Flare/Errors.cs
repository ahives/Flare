namespace Flare;

internal static class Errors
{
    public static Error Create(ErrorType errorType, string reason) =>
        new()
        {
            Reason = reason,
            Type = errorType,
            Timestamp = DateTimeOffset.UtcNow
        };
}