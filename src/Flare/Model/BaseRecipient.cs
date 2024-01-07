namespace Flare.Model;

using System.Text.Json.Serialization;

public record BaseRecipient
{
    [JsonPropertyName("type")]
    public RecipientType Type { get; init; }

    protected BaseRecipient(RecipientType type)
    {
        Type = type;
    }
}