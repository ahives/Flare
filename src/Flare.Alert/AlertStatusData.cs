namespace Flare.Alert;

using System.Text.Json.Serialization;
using Model;

public sealed record AlertStatusData
{
    [JsonPropertyName("alertId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid Id { get; init; }
    
    [JsonPropertyName("integrationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid IntegrationId { get; init; }
    
    [JsonPropertyName("alias")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid Alias { get; init; }
    
    [JsonPropertyName("action")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AlertAction Action { get; init; }
    
    [JsonPropertyName("processedAt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset ProcessedAt { get; init; }
    
    [JsonPropertyName("success")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Success { get; init; }
    
    [JsonPropertyName("isSuccess")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsSuccess { get; init; }
    
    [JsonPropertyName("status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Status { get; init; }
}