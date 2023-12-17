namespace Flare.API.Model;

using System.Text.Json.Serialization;

public record ListAlertResponse
{
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<AbbreviatedAlertData> Data { get; set; }
    
    [JsonPropertyName("paging")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Paging Paging { get; set; }
    
    [JsonPropertyName("took")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public float Took { get; set; }
    
    [JsonPropertyName("requestId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid RequestId { get; set; }
}

public record Paging
{
    [JsonPropertyName("next")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Next { get; set; }
    
    [JsonPropertyName("first")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string First { get; set; }
    
    [JsonPropertyName("last")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Last { get; set; }
}