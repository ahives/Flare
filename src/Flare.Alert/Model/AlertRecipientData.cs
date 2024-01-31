namespace Flare.Alert.Model;

using System.Text.Json.Serialization;

public sealed record AlertRecipientData
{
    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public User User { get; init; }

    [JsonPropertyName("state")]
    public AlertRecipientState State { get; init; }

    [JsonPropertyName("method")]
    public AlertMethod Method { get; init; }

    [JsonPropertyName("createdAt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset CreatedAt { get; init; }

    [JsonPropertyName("updatedAt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset UpdatedAt { get; init; }
}