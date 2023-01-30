namespace Flare;

public record Result
{
    public DateTimeOffset Timestamp { get; init; }

    public DebugInfo DebugInfo { get; init; }

    public bool HasFaulted { get; init; }
    
    public Guid RequestId { get; set; }
    
    public float Took { get; set; }
}

public record Result<T> :
    Result
{
    public T Data { get; init; }

    public bool HasData { get; init; }
}