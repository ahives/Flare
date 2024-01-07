namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertInfo
{
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AlertData Data { get; init; }
}