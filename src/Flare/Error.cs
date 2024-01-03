namespace Flare;

public record Error
{
    public string Reason { get; init; }

    public ErrorType Type { get; init; }

    public DateTimeOffset Timestamp { get; init; }
}