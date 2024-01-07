namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertStatusInfo
{
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AlertStatusData Data { get; init; }

    [JsonPropertyName("took")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float Took { get; init; }

    [JsonPropertyName("requestId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid RequestId { get; init; }
}