namespace Flare.Alert.Serialization;

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
                new AlertActionConverter(),
                new AlertTagConverter(),
                new AlertRecipientStateConverter(),
                new AlertMethodConverter(),
                new AlertLogTypeConverter(),
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
}