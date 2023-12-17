namespace Flare.API.Model;

using System.Text.Json.Serialization;

public record GetAlertResponse
{
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AlertData Data { get; set; }
}