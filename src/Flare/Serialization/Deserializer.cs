using System.Text.Json;
using System.Text.Json.Serialization;
using Flare.Serialization.Converters;

namespace Flare.Serialization;

public static class Deserializer
{
    public static JsonSerializerOptions Options =>
        new()
        {
            WriteIndented = true,
            Converters =
            {
                new AlertStatusConverter(),
                new ApiIntegrationTypeConverter(),
                new RecipientTypeConverter(),
                new AlertPriorityConverter(),
                new CustomFloatConverter(),
                new CustomDecimalConverter(),
                new CustomDateTimeConverter(),
                new CustomLongConverter(),
                new CustomStringConverter(),
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
}