namespace Flare.Alert.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Model;

public class AlertMethodConverter :
    JsonConverter<AlertMethod>
{
    public override AlertMethod Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "Acknowledge" => AlertMethod.Acknowledge,
            "" => AlertMethod.NotApplicable,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, AlertMethod value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case AlertMethod.Acknowledge:
                writer.WriteStringValue("Acknowledge");
                break;

            default:
                throw new JsonException();
        }
    }
}