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
        HasData = Data is not null;
        HasFaulted = false;
        Timestamp = DateTimeOffset.UtcNow;
    }
}