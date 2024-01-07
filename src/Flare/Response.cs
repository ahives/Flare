namespace Flare;

using Model;

public static class Response
{
    public static Maybe<IReadOnlyList<T>> Success<T>(IReadOnlyList<T> data, DebugInfo debugInfo) =>
        new()
        {
            Result = data,
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = false
        };

    public static Maybe<T> Success<T>(T data, DebugInfo debugInfo) =>
        new()
        {
            Result = data,
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = false
        };

    public static Maybe Success(DebugInfo debugInfo) =>
        new()
        {
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = false
        };

    public static Maybe<IReadOnlyList<T>> Failures<T>(DebugInfo debugInfo) =>
        new()
        {
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = true
        };

    public static Maybe<T> Failed<T>(DebugInfo debugInfo) =>
        new()
        {
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = true
        };

    public static Maybe Failed(DebugInfo debugInfo) =>
        new()
        {
            DebugInfo = debugInfo,
            Timestamp = DateTimeOffset.UtcNow,
            HasFaulted = true
        };
}