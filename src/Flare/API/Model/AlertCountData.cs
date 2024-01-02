namespace Flare.API.Model;

using System.Text.Json.Serialization;

public record AlertCountData
{
    [JsonPropertyName("count")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Count { get; init; }
}