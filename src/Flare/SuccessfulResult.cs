namespace Flare;

public record SuccessfulResult :
    Result
{
    public SuccessfulResult()
    {
        HasFaulted = false;
        Timestamp = DateTimeOffset.UtcNow;
    }
}

public record SuccessfulResult<T> :
    Result<T>
{
    public SuccessfulResult()
    {
        HasFaulted = false;
        Timestamp = DateTimeOffset.UtcNow;
    }
}