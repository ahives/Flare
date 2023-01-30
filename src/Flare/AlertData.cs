namespace Flare;

public class AlertData
{
    public Guid Id { get; set; }
    
    public Guid IntegerationId { get; set; }
    
    public Guid Alias { get; set; }
    
    public string Action { get; set; }
    
    public DateTimeOffset ProcessedAt { get; set; }
    
    public bool Success { get; set; }
    
    public bool IsSuccess { get; set; }
    
    public string Status { get; set; }
}