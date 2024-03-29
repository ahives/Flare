namespace Flare.Model;

using System.Text.Json.Serialization;

public sealed record ResultInfo
{
    [JsonPropertyName("result")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Result { get; init; }

    [JsonPropertyName("took")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float Took { get; init; }

    [JsonPropertyName("requestId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid RequestId { get; init; }
}