namespace Flare;

using Extensions;

public record Result
{
    public DateTimeOffset Timestamp { get; init; }

    public DebugInfo? DebugInfo { get; init; }

    public bool HasFaulted { get; init; }
}

public record Result<T> :
    Result
{
    public T? Data { get; init; }

    public bool HasData => Data is not null;
}