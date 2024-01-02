namespace Flare.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;
using Converters;

public static class Serializer
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
                new RespondersTypeConverter(),
                new ResponderTypeConverter(),
                new AlertActionConverter(),
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
}