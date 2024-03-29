namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record Paging
{
    [JsonPropertyName("next")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Next { get; init; }

    [JsonPropertyName("first")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? First { get; init; }

    [JsonPropertyName("last")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Last { get; init; }
}