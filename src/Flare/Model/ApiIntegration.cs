namespace Flare.Model;

using System.Text.Json.Serialization;

public sealed record ApiIntegration
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public ApiIntegrationType Type { get; set; }
}