namespace Flare.Alert.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Model;

public class AlertStatusConverter :
    JsonConverter<AlertStatus>
{
    public override AlertStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "closed" => AlertStatus.Closed,
            "open" => AlertStatus.Open,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, AlertStatus value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case AlertStatus.Closed:
                writer.WriteStringValue("API");
                break;

            default:
                throw new JsonException();
        }
    }
}