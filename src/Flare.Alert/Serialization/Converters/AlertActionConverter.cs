namespace Flare.Alert.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;

public class AlertActionConverter :
    JsonConverter<AlertAction>
{
    public override AlertAction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "Create" => AlertAction.Create,
            "Acknowledge" => AlertAction.Acknowledge,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, AlertAction value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case AlertAction.Create:
                writer.WriteStringValue("Create");
                break;

            case AlertAction.Acknowledge:
                writer.WriteStringValue("Acknowledge");
                break;

            default:
                throw new JsonException("Invalid AlertAction value. Unable to convert to JSON.");
        }
    }
}