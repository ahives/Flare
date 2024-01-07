namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertStatusData
{
    [JsonPropertyName("success")]
    public bool Success { get; init; }

    [JsonPropertyName("action")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AlertAction Action { get; init; }

    [JsonPropertyName("processedAt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset ProcessedAt { get; init; }

    [JsonPropertyName("integrationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid IntegrationId { get; init; }

    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; init; }

    [JsonPropertyName("status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Status { get; init; }

    [JsonPropertyName("alertId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string AlertId { get; init; }

    [JsonPropertyName("alias")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Alias { get; init; }
}