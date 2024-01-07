namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertCountData
{
    [JsonPropertyName("count")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Count { get; init; }
}