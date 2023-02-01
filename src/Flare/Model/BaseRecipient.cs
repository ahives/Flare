using System.Text.Json.Serialization;

namespace Flare.Model;

public record BaseRecipient
{
    [JsonPropertyName("type")]
    public RecipientType Type { get; init; }

    protected BaseRecipient(RecipientType type)
    {
        Type = type;
    }
}