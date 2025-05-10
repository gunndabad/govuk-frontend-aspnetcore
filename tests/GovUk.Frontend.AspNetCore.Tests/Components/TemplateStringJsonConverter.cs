using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GovUk.Frontend.AspNetCore.Components;

namespace GovUk.Frontend.AspNetCore.Tests.Components;

public class TemplateStringJsonConverter : JsonConverter<TemplateString>
{
    public override TemplateString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType is JsonTokenType.Number or JsonTokenType.String or JsonTokenType.False or JsonTokenType.True)
        {
            return new TemplateString(JsonSerializer.Deserialize(ref reader, typeof(string), options) as string);
        }

        throw new NotSupportedException($"Cannot create a {nameof(TemplateString)} from a {reader.TokenType}.");
    }

    public override void Write(Utf8JsonWriter writer, TemplateString value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
