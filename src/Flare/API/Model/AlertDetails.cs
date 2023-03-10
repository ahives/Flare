using System.Text.Json.Serialization;

namespace Flare.API.Model;

public record AlertDetails
{
    [JsonPropertyName("serverName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ServerName { get; set; }

    [JsonPropertyName("region")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Region { get; set; }
}