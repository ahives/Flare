namespace Flare.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using API.Model;

public class ResponderTypeConverter :
    JsonConverter<ResponderType>
{
    public override ResponderType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "team" => ResponderType.Team,
            "escalation" => ResponderType.Escalation,
            "user" => ResponderType.User,
            "schedule" => ResponderType.Schedule,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, ResponderType value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ResponderType.Team:
                writer.WriteStringValue("team");
                break;
            
            case ResponderType.User:
                writer.WriteStringValue("user");
                break;
            
            case ResponderType.Escalation:
                writer.WriteStringValue("escalation");
                break;
            
            case ResponderType.Schedule:
                writer.WriteStringValue("schedule");
                break;
            
            default:
                throw new JsonException();
        }
    }
}