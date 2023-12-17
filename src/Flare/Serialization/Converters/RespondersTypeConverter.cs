namespace Flare.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using API.Model;

public class RespondersTypeConverter :
    JsonConverter<List<object>>
{
    public override List<object>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var responders = new List<object>();
        var value = JsonDocument.ParseValue(ref reader);
        if (value.RootElement.ValueKind != JsonValueKind.Array)
            return responders;

        foreach (var element in value.RootElement.EnumerateArray())
        {
            if (element.ValueKind != JsonValueKind.Object)
                continue;

            var properties = element.EnumerateObject();
            if (properties.Any(x => x.Name.Equals("id")))
            {
                var responder = JsonSerializer.Deserialize<TeamIdResponder>(element.GetRawText(), options);
                if (responder is not null)
                    responders.Add(responder);
            }
            else if (properties.Any(x => x.Name.Equals("name")))
            {
                var responder = JsonSerializer.Deserialize<TeamNameResponder>(element.GetRawText(), options);
                if (responder is not null)
                    responders.Add(responder);
            }
            else if (properties.Any(x => x.Name.Equals("username")))
            {
                var responder = JsonSerializer.Deserialize<UserResponder>(element.GetRawText(), options);
                if (responder is not null)
                    responders.Add(responder);
            }
        }

        return responders;
    }

    public override void Write(Utf8JsonWriter writer, List<object> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

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