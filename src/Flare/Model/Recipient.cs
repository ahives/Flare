using System.Text.Json.Serialization;

namespace Flare.Model;

public record Recipient :
    BaseRecipient
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid Id { get; }

    private Recipient(Guid id, RecipientType type) : base(type)
    {
        Id = id;
    }

    public static Recipient Add(Guid id, RecipientType type)
    {
        return new Recipient(id, type);
    }
}