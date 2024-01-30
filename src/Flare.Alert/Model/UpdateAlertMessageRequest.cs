namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record UpdateAlertMessageRequest
{
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Message { get; init; }
}