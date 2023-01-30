using System.Text.Json.Serialization;

namespace Flare;

public record BaseRecipient
{
    [JsonPropertyName("type")]
    public RecipientType Type { get; init; }

    protected BaseRecipient(RecipientType type)
    {
        Type = type;
    }
}