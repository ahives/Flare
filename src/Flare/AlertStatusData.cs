namespace Flare;

public record AlertStatusData
{
    public Guid Id { get; set; }
    
    public Guid IntegrationId { get; set; }
    
    public Guid Alias { get; set; }
    
    public string Action { get; set; }
    
    public DateTimeOffset ProcessedAt { get; set; }
    
    public bool Success { get; set; }
    
    public bool IsSuccess { get; set; }
    
    public string Status { get; set; }
}