namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record EscalateAlertRequest
{
    [JsonPropertyName("escalation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Escalation? Escalation { get; init; }

    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string User { get; init; }

    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Source { get; init; }

    [JsonPropertyName("note")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Note { get; init; }
}