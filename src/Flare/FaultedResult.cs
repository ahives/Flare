namespace Flare;

public record FaultedResult :
    Result
{
    public FaultedResult()
    {
        HasFaulted = true;
        Timestamp = DateTimeOffset.UtcNow;
    }
}

public record FaultedResult<T> :
    Result<T>
{
    public FaultedResult()
    {
        Data = default!;
        HasFaulted = true;
        Timestamp = DateTimeOffset.UtcNow;
    }
}