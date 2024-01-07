namespace Flare.Alert.Model;

using System.Text.Json.Serialization;
using Flare.Model;

public record AbbreviatedAlertData
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid Id { get; init; }

    [JsonPropertyName("tinyId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string TinyId { get; init; }

    [JsonPropertyName("alias")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Alias { get; init; }

    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Message { get; init; }

    [JsonPropertyName("status")]
    public AlertStatus Status { get; init; }

    [JsonPropertyName("acknowledged")]
    public bool Acknowledged { get; init; }

    [JsonPropertyName("isSeen")]
    public bool IsSeen { get; init; }

    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> Tags { get; init; }

    [JsonPropertyName("snoozed")]
    public bool Snoozed { get; init; }

    [JsonPropertyName("snoozedUntil")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset SnoozedUntil { get; init; }

    [JsonPropertyName("count")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Count { get; init; }

    [JsonPropertyName("lastOccurredAt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset LastOccurredAt { get; init; }

    [JsonPropertyName("createdAt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset CreatedAt { get; init; }

    [JsonPropertyName("updatedAt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTimeOffset UpdatedAt { get; init; }

    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Source { get; init; }

    [JsonPropertyName("owner")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Owner { get; init; }

    [JsonPropertyName("priority")]
    public AlertPriority Priority { get; init; }

    [JsonPropertyName("responders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<object> Responders { get; init; }

    [JsonPropertyName("integration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ApiIntegration Integration { get; init; }

    [JsonPropertyName("report")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AlertReport Report { get; init; }
}



public record TeamIdResponder
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid Id { get; init; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ResponderType Type { get; init; }
}

public record TeamNameResponder
{
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Name { get; init; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ResponderType Type { get; init; }
}

public record UserResponder
{
    [JsonPropertyName("username")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Username { get; init; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ResponderType Type { get; init; }
}

public enum ResponderType
{
    Team,
    User,
    Escalation,
    Schedule
}
