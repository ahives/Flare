namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record UpdateAlertPriorityRequest
{
    [JsonPropertyName("priority")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AlertPriority Priority { get; init; }
}