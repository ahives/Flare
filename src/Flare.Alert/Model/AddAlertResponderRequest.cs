namespace Flare.Alert.Model;

using System.Text.Json.Serialization;
using Flare.Model;

public sealed record AddAlertResponderRequest
{
    [JsonPropertyName("responder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Responder? Responder { get; init; }

    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string User { get; init; }

    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Source { get; init; }

    [JsonPropertyName("note")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Notes { get; init; }
}