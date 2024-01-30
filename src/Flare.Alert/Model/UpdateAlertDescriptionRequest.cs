namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record UpdateAlertDescriptionRequest
{
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Description { get; init; }
}