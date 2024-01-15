namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AddAlertTagsRequest
{
    [JsonPropertyName("responder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> Tags { get; init; }

    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string User { get; init; }

    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Source { get; init; }

    [JsonPropertyName("note")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Notes { get; init; }
}