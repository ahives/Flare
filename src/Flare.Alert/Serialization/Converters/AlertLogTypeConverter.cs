namespace Flare.Alert.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Model;

public class AlertLogTypeConverter :
    JsonConverter<AlertLogType>
{
    public override AlertLogType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "system" => AlertLogType.System,
            "alertRecipient" => AlertLogType.AlertRecipient,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, AlertLogType value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case AlertLogType.System:
                writer.WriteStringValue("system");
                break;

            case AlertLogType.AlertRecipient:
                writer.WriteStringValue("alertRecipient");
                break;

            default:
                throw new JsonException();
        }
    }
}