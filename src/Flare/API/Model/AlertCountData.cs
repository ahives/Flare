using System.Text.Json.Serialization;

namespace Flare.API.Model;

public record AlertCountData
{
    [JsonPropertyName("count")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Count { get; set; }
}