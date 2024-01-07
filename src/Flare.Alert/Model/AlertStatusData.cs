namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertStatusData
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("action")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Action { get; set; }

    [JsonPropertyName("processedAt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset ProcessedAt { get; set; }

    [JsonPropertyName("integrationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid IntegrationId { get; set; }

    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Status { get; set; }

    [JsonPropertyName("alertId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid AlertId { get; set; }

    [JsonPropertyName("alias")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Alias { get; set; }
}