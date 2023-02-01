using System.Text.Json.Serialization;

namespace Flare.Model;

public record CreateAlertRequest
{
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Message { get; set; }

    [JsonPropertyName("alias")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Alias { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Description { get; set; }

    [JsonPropertyName("responders")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object[] Responders { get; set; }

    [JsonPropertyName("visibleTo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object[] VisibleTo { get; set; }

    [JsonPropertyName("actions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> Actions { get; set; }

    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> Tags { get; set; }

    [JsonPropertyName("details")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IDictionary<string, string> Details { get; set; }
    
    [JsonPropertyName("entity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Entity { get; set; }

    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Source { get; set; }

    [JsonPropertyName("priority")]
    public AlertPriority Priority { get; set; }
    
    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string User { get; set; }
    
    [JsonPropertyName("note")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Note { get; set; }
}