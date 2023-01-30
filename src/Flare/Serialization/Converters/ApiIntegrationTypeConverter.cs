using System.Text.Json;
using System.Text.Json.Serialization;

namespace Flare.Serialization.Converters;

public class ApiIntegrationTypeConverter :
    JsonConverter<ApiIntegrationType>
{
    public override ApiIntegrationType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "API" => ApiIntegrationType.API,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, ApiIntegrationType value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ApiIntegrationType.API:
                writer.WriteStringValue("API");
                break;

            default:
                throw new JsonException();
        }
    }
}