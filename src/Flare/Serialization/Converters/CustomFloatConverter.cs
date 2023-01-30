using System.Text.Json;
using System.Text.Json.Serialization;

namespace Flare.Serialization.Converters;

public class CustomFloatConverter :
    JsonConverter<float>
{
    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                string stringValue = reader.GetString()!;
                if (float.TryParse(stringValue, out var value))
                    return value;
                break;

            case JsonTokenType.Number:
                return reader.GetSingle();

            case JsonTokenType.Null:
                return default;
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value);
}