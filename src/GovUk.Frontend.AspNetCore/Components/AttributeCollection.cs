using System.Collections;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Components;

/// <summary>
/// Represents a collection of HTML attributes.
/// </summary>
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

        foreach (var tagHelperAttribute in tagHelperAttributes)
        {
            var attribute = new Attribute(
                tagHelperAttribute.Name,
                tagHelperAttribute.Value,
                Optional: tagHelperAttribute.ValueStyle is HtmlAttributeValueStyle.Minimized);

            _attributes.Add(attribute.Name, attribute);
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

    internal void Add(string name, TemplateString templateString)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(templateString);

        var attribute = new Attribute(name, templateString, Optional: false);
        _attributes.Add(name, attribute);
    }

    internal void Set(string name, TemplateString? templateString)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (templateString is null)
        {
            _attributes.Remove(name);
            return;
        }

        var attribute = new Attribute(name, templateString, Optional: false);
        _attributes[name] = attribute;
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

    internal sealed record Attribute(string Name, object? Value, bool Optional)
    {
        public string GetValueHtmlString(HtmlEncoder encoder)
        {
            ArgumentNullException.ThrowIfNull(encoder);

            if (Value is TemplateString templateString)
            {
                return templateString.ToHtmlString(encoder);
            }

            if (Value is IHtmlContent htmlContent)
            {
                return htmlContent.ToHtmlString(encoder);
            }

            return Value?.ToString() ?? string.Empty;
        }
    }

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
