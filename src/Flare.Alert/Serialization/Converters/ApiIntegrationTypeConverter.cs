namespace Flare.Alert.Serialization.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Model;

public class ApiIntegrationTypeConverter :
    JsonConverter<ApiIntegrationType>
{
    public override ApiIntegrationType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "API" => ApiIntegrationType.API,
            "CloudWatch" => ApiIntegrationType.CloudWatch,
            _ => throw new JsonException()
        };

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