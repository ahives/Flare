namespace Flare.Model;

using System.Text.Json.Serialization;

public record UserRecipient :
    BaseRecipient
{
    [JsonPropertyName("username")]
    public string Username { get; }

    private UserRecipient(string username, RecipientType type) : base(type)
    {
        Username = username;
    }

    public static UserRecipient Add(string username, RecipientType type)
    {
        return new UserRecipient(username, type);
    }
}