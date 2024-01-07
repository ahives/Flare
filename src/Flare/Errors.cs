namespace Flare;

using Model;

public static class Errors
{
    public static Error Create(ErrorType errorType, string reason) =>
        new()
        {
            Reason = reason,
            Type = errorType,
            Timestamp = DateTimeOffset.UtcNow
        };
}