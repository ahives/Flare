using System.Text.Json.Serialization;

namespace Flare.Model;

public record TeamRecipient :
    BaseRecipient
{
    [JsonPropertyName("name")]
    public string Name { get; }

    private TeamRecipient(string name, RecipientType type) : base(type)
    {
        Name = name;
    }

    public static TeamRecipient Add(string name, RecipientType type)
    {
        return new TeamRecipient(name, type);
    }
}