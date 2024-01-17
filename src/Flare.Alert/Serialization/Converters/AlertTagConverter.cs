namespace Flare.Alert.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;

public class AlertTagConverter :
    JsonConverter<AlertTag>
{
    public override AlertTag Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "OverwriteQuietHours" => AlertTag.OverwriteQuietHours,
            "Critical" => AlertTag.Critical,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, AlertTag value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case AlertTag.OverwriteQuietHours:
                writer.WriteStringValue("OverwriteQuietHours");
                break;

            case AlertTag.Critical:
                writer.WriteStringValue("Critical");
                break;
            
            default:
                throw new JsonException();
        }
    }
}