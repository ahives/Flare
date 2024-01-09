namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record ApiIntegration
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Name { get; init; }

    [JsonPropertyName("type")]
    public ApiIntegrationType Type { get; init; }
}