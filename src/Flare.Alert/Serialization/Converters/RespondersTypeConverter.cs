namespace Flare.Alert.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Model;

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