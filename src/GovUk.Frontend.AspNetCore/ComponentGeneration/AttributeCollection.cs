using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// Represents a collection of HTML attributes.
/// </summary>
[JsonConverter(typeof(AttributeCollectionJsonConverter))]
public sealed class AttributeCollection : IEnumerable<KeyValuePair<string, string?>>
{
    private readonly Dictionary<string, Attribute> _attributes;

    /// <summary>
    /// Initializes a new emty instance of the <see cref="AttributeCollection"/> class.
    /// </summary>
    public AttributeCollection()
    {
        _attributes = new();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeCollection"/> class
    /// with the attributes in the specified <paramref name="attributes"/>.
    /// </summary>
    /// <param name="attributes">The existing attributes.</param>
    public AttributeCollection(IDictionary<string, string?>? attributes)
    {
        if (attributes is null)
        {
            _attributes = new();
            return;
        }

        _attributes = attributes.ToDictionary(a => a.Key, a => new Attribute(a.Key, a.Value, Optional: false));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeCollection"/> class
    /// with the attributes in the specified <paramref name="tagHelperAttributes"/>.
    /// </summary>
    /// <param name="tagHelperAttributes">The <see cref="TagHelperAttribute" />s to copy attributes from.</param>
    public AttributeCollection(IEnumerable<TagHelperAttribute> tagHelperAttributes)
    {
        ArgumentNullException.ThrowIfNull(tagHelperAttributes);

        _attributes = new();

        foreach (var attribute in tagHelperAttributes)
        {
            var attributeValue = attribute.Value is IHtmlContent htmlContent ? WebUtility.HtmlDecode(htmlContent.ToHtmlString()) : attribute.Value;

            var encodedAttribute = new Attribute(
                attribute.Name,
                attributeValue,
                Optional: attribute.ValueStyle is HtmlAttributeValueStyle.Minimized);

            _attributes.Add(encodedAttribute.Name, encodedAttribute);
        }
    }

    internal AttributeCollection(IEnumerable<Attribute> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        _attributes = attributes.ToDictionary(a => a.Name, a => a);
    }

    internal AttributeCollection(params Attribute[] attributes) : this(attributes.AsEnumerable())
    {
    }

    /// <summary>
    /// Creates a new <see cref="AttributeCollection"/> with a copy of the attributes from this instance.
    /// </summary>
    /// <returns>A new <see cref="AttributeCollection"/>.</returns>
    public AttributeCollection Clone() =>
        new(_attributes.Values.Select(a => new Attribute(a.Name, a.Value, a.Optional)));

    internal IReadOnlyCollection<Attribute> GetAttributes() => _attributes.Values;

    internal bool Remove(string name, out string? value)
    {
        if (_attributes.Remove(name, out var attribute))
        {
            value = attribute.Value?.ToString();
            return true;
        }

        value = null;
        return false;
    }

    internal sealed record Attribute(string Name, object? Value, bool Optional);

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

        var attributes = new List<AttributeCollection.Attribute>();

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
