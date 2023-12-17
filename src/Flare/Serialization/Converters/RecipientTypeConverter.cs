namespace Flare.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;

public class RecipientTypeConverter :
    JsonConverter<RecipientType>
{
    public override RecipientType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "team" => RecipientType.Team,
            "escalation" => RecipientType.Escalation,
            "user" => RecipientType.User,
            "schedule" => RecipientType.Schedule,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, RecipientType value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case RecipientType.Team:
                writer.WriteStringValue("team");
                break;
            
            case RecipientType.User:
                writer.WriteStringValue("user");
                break;
            
            case RecipientType.Escalation:
                writer.WriteStringValue("escalation");
                break;
            
            case RecipientType.Schedule:
                writer.WriteStringValue("schedule");
                break;
            
            default:
                throw new JsonException();
        }
    }
}