namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertLogData
{
    [JsonPropertyName("log")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Log { get; init; }

    [JsonPropertyName("type")]
    public AlertLogType Type { get; init; }

    [JsonPropertyName("createdAt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset CreatedAt { get; init; }

    [JsonPropertyName("owner")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Owner { get; init; }

    [JsonPropertyName("offset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Offset { get; init; }
}