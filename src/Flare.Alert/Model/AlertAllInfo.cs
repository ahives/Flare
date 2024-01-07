namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertAllInfo
{
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<AbbreviatedAlertData>? Data { get; init; }

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