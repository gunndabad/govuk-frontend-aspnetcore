using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// Represents a collection of HTML attributes.
/// </summary>
[JsonConverter(typeof(AttributeCollectionJsonConverter))]
public sealed class AttributeCollection : IEnumerable<KeyValuePair<string, string?>>
{
    private readonly Dictionary<string, EncodedAttribute> _attributes;

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeCollection"/> class.
    /// </summary>
    public AttributeCollection()
    {
        _attributes = new();
    }

    internal AttributeCollection(IEnumerable<TagHelperAttribute> tagHelperAttributes)
    {
        ArgumentNullException.ThrowIfNull(tagHelperAttributes);

        _attributes = new();

        foreach (var attribute in tagHelperAttributes)
        {
            var encodedAttribute = new EncodedAttribute(
                attribute.Name,
                attribute.Value switch
                {
                    _ when attribute.ValueStyle is HtmlAttributeValueStyle.Minimized => true,
                    IHtmlContent htmlContent => htmlContent.ToHtmlString(),
                    string str => str,
                    var obj => obj?.ToString()
                },
                Optional: attribute.ValueStyle is HtmlAttributeValueStyle.Minimized);

            _attributes.Add(encodedAttribute.Name, encodedAttribute);
        }
    }

    internal AttributeCollection(IEnumerable<EncodedAttribute> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        _attributes = attributes.ToDictionary(a => a.Name, a => a);
    }

    internal IReadOnlyCollection<EncodedAttribute> GetAttributes() => _attributes.Values;

    internal bool Remove(string name, out string? value)
    {
        if (_attributes.Remove(name, out var attribute))
        {
            value = attribute.Value as string;
            return true;
        }

        value = null;
        return false;
    }

    internal sealed record EncodedAttribute(string Name, object? Value, bool Optional);

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<KeyValuePair<string, string?>> GetEnumerator()
    {
        foreach (var attribute in _attributes.Values)
        {
            if (attribute.Optional)
            {
                if (attribute.Value is true)
                {
                    yield return KeyValuePair.Create(attribute.Name, (string?)null);
                }

                continue;
            }

            yield return KeyValuePair.Create(attribute.Name, attribute.Value?.ToString());
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class AttributeCollectionJsonConverter : JsonConverter<AttributeCollection>
{
    public override AttributeCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType is not JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var attributes = new List<AttributeCollection.EncodedAttribute>();

        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndObject)
            {
                return new AttributeCollection(attributes);
            }

            if (reader.TokenType is not JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            var name = reader.GetString()!;
            object? value = null;
            var optional = false;

            reader.Read();

            if (reader.TokenType is JsonTokenType.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType is JsonTokenType.EndObject)
                    {
                        break;
                    }

                    if (reader.TokenType is not JsonTokenType.PropertyName)
                    {
                        throw new JsonException();
                    }

                    var propertyName = reader.GetString();
                    reader.Read();
                    if (propertyName is "value")
                    {
                        value = ReadAttributeValue(ref reader);
                    }
                    else if (propertyName is "optional")
                    {
                        optional = reader.GetBoolean();
                    }
                    else if (propertyName is "type")
                    {
                    }
                    else
                    {
                        throw new JsonException();
                    }
                }
            }
            else
            {
                value = ReadAttributeValue(ref reader);
            }

            attributes.Add(new(name, value, optional));
        }

        throw new JsonException();

        static object? ReadAttributeValue(ref Utf8JsonReader reader) => reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Number => reader.GetDecimal(),
            JsonTokenType.False or JsonTokenType.True => reader.GetBoolean(),
            JsonTokenType.Null => null,
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, AttributeCollection value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var attr in value.GetAttributes())
        {
            writer.WritePropertyName(attr.Name);

            if (attr.Optional)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("value");
                JsonSerializer.Serialize(writer, attr.Value, options);
                writer.WriteBoolean("optional", attr.Optional);
                writer.WriteEndObject();
            }
            else
            {
                JsonSerializer.Serialize(writer, attr.Value, options);
            }
        }

        writer.WriteEndObject();
    }
}
