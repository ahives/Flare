namespace Flare.Alert.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Model;

public class AlertRecipientStateConverter :
    JsonConverter<AlertRecipientState>
{
    public override AlertRecipientState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "action" => AlertRecipientState.Action,
            "notactive" => AlertRecipientState.NotActive,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, AlertRecipientState value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case AlertRecipientState.Action:
                writer.WriteStringValue("action");
                break;

            case AlertRecipientState.NotActive:
                writer.WriteStringValue("notactive");
                break;

            default:
                throw new JsonException();
        }
    }
}