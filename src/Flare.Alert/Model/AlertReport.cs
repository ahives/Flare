namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertReport
{
    [JsonPropertyName("ackTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long AckTime { get; set; }

    [JsonPropertyName("closeTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long CloseTime { get; set; }

    [JsonPropertyName("acknowledgedBy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string AcknowledgedBy { get; set; }

    [JsonPropertyName("closedBy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ClosedBy { get; set; }
}