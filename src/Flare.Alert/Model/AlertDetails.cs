namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertDetails
{
    [JsonPropertyName("serverName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ServerName { get; init; }

    [JsonPropertyName("region")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Region { get; init; }
}