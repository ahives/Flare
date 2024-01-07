namespace Flare;

using Model;

public record Maybe
{
    public DateTimeOffset Timestamp { get; init; }

    public DebugInfo? DebugInfo { get; init; }

    public bool HasFaulted { get; init; }
}

public record Maybe<T> :
    Maybe
{
    public T? Result { get; init; }

    public bool HasResult => Result is not null;
}