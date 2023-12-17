namespace Flare;

internal static class Response
{
    public static Result<IReadOnlyList<T>> Success<T>(IReadOnlyList<T> data, DebugInfo debugInfo) =>
        new()
        {
            Data = data,
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = false
        };

    public static Result<T> Success<T>(T data, DebugInfo debugInfo) =>
        new()
        {
            Data = data,
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = false
        };

    public static Result Success(DebugInfo debugInfo) =>
        new()
        {
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = false
        };

    public static Result<IReadOnlyList<T>> Failures<T>(DebugInfo debugInfo) =>
        new()
        {
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = true
        };

    public static Result<T> Failed<T>(DebugInfo debugInfo) =>
        new()
        {
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = true
        };

    public static Result Failed(DebugInfo debugInfo) =>
        new()
        {
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = true
        };
}