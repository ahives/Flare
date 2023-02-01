using System.Text.Json;
using System.Text.Json.Serialization;
using Flare.Model;

namespace Flare.Serialization.Converters;

public class AlertPriorityConverter :
    JsonConverter<AlertPriority>
{
    public override AlertPriority Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "P1" => AlertPriority.P1,
            "P2" => AlertPriority.P2,
            "P3" => AlertPriority.P3,
            "P4" => AlertPriority.P4,
            "P5" => AlertPriority.P5,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, AlertPriority value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case AlertPriority.P1:
                writer.WriteStringValue("P1");
                break;
            
            case AlertPriority.P2:
                writer.WriteStringValue("P2");
                break;
            
            case AlertPriority.P3:
                writer.WriteStringValue("P3");
                break;
            
            case AlertPriority.P4:
                writer.WriteStringValue("P4");
                break;
            
            case AlertPriority.P5:
                writer.WriteStringValue("P5");
                break;
            
            default:
                throw new JsonException();
        }
    }
}