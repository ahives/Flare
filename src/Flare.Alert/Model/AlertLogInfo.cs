namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertLogInfo
{
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<AlertLogData>? Data { get; init; }

    [JsonPropertyName("paging")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Paging? Paging { get; init; }

    [JsonPropertyName("took")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float Took { get; init; }

    [JsonPropertyName("requestId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid RequestId { get; init; }
}