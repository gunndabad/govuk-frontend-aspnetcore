using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Components;

/// <summary>
/// Represents a collection of HTML attributes and their values.
/// </summary>
public class EncodedAttributesDictionary : IReadOnlyDictionary<string, string>
{
    private const string BooleanAttributeValue = "";

    /// <summary>
    /// The underlying collection of attributes.
    /// </summary>
    /// <remarks>
    /// We use <see cref="TagHelperAttributeList"/> since it supports pre-encoded values as well as multiple styles
    /// e.g. Minimized (i.e. for boolean attributes).
    /// </remarks>
    private readonly TagHelperAttributeList _attributes;

    /// <summary>
    /// Creates a new <see cref="EncodedAttributesDictionary"/>.
    /// </summary>
    public EncodedAttributesDictionary()
    {
        _attributes = new TagHelperAttributeList();
    }

    /// <summary>
    /// Creates a new <see cref="EncodedAttributesDictionary"/> from an existing <see cref="ReadOnlyTagHelperAttributeList"/>.
    /// </summary>
    /// <param name="tagHelperAttributes">The <see cref="ReadOnlyTagHelperAttributeList"/>to copy attributes from.</param>
    public EncodedAttributesDictionary(ReadOnlyTagHelperAttributeList tagHelperAttributes)
    {
        ArgumentNullException.ThrowIfNull(tagHelperAttributes);
        _attributes = new TagHelperAttributeList(tagHelperAttributes);
    }

    /// <inheritdoc/>
    public int Count => _attributes.Count;

    IEnumerable<string> IReadOnlyDictionary<string, string>.Keys => throw new NotImplementedException();

    IEnumerable<string> IReadOnlyDictionary<string, string>.Values => throw new NotImplementedException();

    int IReadOnlyCollection<KeyValuePair<string, string>>.Count => throw new NotImplementedException();

    string IReadOnlyDictionary<string, string>.this[string key] => throw new NotImplementedException();

    /// <summary>
    /// Creates an <see cref="EncodedAttributesDictionary"/> from a <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <remarks>
    /// Values must be HTML encoded.
    /// </remarks>
    /// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> to copy attributes from.</param>
    /// <returns>A new <see cref="EncodedAttributesDictionary"/>.</returns>
    public static EncodedAttributesDictionary FromDictionaryWithEncodedValues(IDictionary<string, string?> dictionary) =>
        new EncodedAttributesDictionary(
            new TagHelperAttributeList(dictionary.Select(
                kvp => kvp.Value is not null ? new TagHelperAttribute(kvp.Key, new HtmlString(kvp.Value)) : new TagHelperAttribute(kvp.Key))));

    /// <summary>
    /// Creates a copy of this <see cref="EncodedAttributesDictionary"/>.
    /// </summary>
    /// <returns>The copied <see cref="EncodedAttributesDictionary"/>.</returns>
    public EncodedAttributesDictionary Clone() => new(_attributes);

    /// <summary>
    /// Adds a boolean attribute.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="ArgumentException" />
    public void AddBoolean(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (_attributes.ContainsName(name))
        {
            throw new ArgumentException($"An attribute with the specified name has already been added. Name: '{name}'.", nameof(name));
        }

        _attributes.Add(new TagHelperAttribute(name));
    }

    /// <summary>
    /// Adds the attributes from another <see cref="EncodedAttributesDictionary"/> to this one.
    /// </summary>
    /// <param name="other">The <see cref="EncodedAttributesDictionary"/> to copy attributes from.</param>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="ArgumentException" />
    public void Add(EncodedAttributesDictionary other)
    {
        ArgumentNullException.ThrowIfNull(other);

        foreach (var attr in other._attributes)
        {
            if (_attributes.ContainsName(attr.Name))
            {
                throw new ArgumentException($"An attribute with the specified name has already been added. Name: '{attr.Name}'.", nameof(other));
            }

            _attributes.Add(attr);
        }
    }

    /// <summary>
    /// Adds an attribute with a value.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="ArgumentException" />
    public void Add(string name, IHtmlContent value)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(value);

        if (_attributes.ContainsName(name))
        {
            throw new ArgumentException($"An attribute with the specified name has already been added. Name: '{name}'.", nameof(name));
        }

        _attributes.Add(new TagHelperAttribute(name, value));
    }

    /// <summary>
    /// Adds an attribute with a value.
    /// </summary>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    /// <param name="encodeValue">Whether the value should be HTML encoded.</param>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="ArgumentException" />
    public void Add(string name, string value, bool encodeValue)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(value);

        if (_attributes.ContainsName(name))
        {
            throw new ArgumentException($"An attribute with the specified name has already been added. Name: '{name}'.", nameof(name));
        }

        _attributes.Add(CreateAttribute(name, value, encodeValue));
    }

    /// <summary>
    /// Adds a CSS class to the attributes.
    /// </summary>
    /// <param name="className">The CSS class to add.</param>
    public void AddCssClass(string className)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(className);

        AddCssClasses([className]);
    }

    /// <summary>
    /// Adds CSS classes to the attributes.
    /// </summary>
    /// <param name="classNames">The CSS classes to add.</param>
    public void AddCssClasses(IEnumerable<string> classNames)
    {
        var newValue = string.Join(' ', classNames);

        if (_attributes.TryGetAttribute("class", out var currentValue))
        {
            newValue = GetEncodedAttributeValue(currentValue) + " " + newValue;
        }

        if (newValue.Length > 0)
        {
            _attributes.SetAttribute(new TagHelperAttribute("class", newValue));
        }
    }

    /// <summary>
    /// Removes the attribute with the specified name, if it exists.
    /// </summary>
    /// <param name="name">The name of the attribute to remove.</param>
    /// <returns><see langword="true"/> if the attribute is successfully found and removed.</returns>
    /// <exception cref="ArgumentNullException" />
    public bool Remove(string name) => Remove(name, out _);

    /// <summary>
    /// Removes the attribute with the specified name and outs its encoded value, if it exists.
    /// </summary>
    /// <remarks>
    /// Boolean attributes will have an <paramref name="encodedValue"/> of an empty string.
    /// </remarks>
    /// <param name="name">The name of the attribute to remove.</param>
    /// <param name="encodedValue">The encoded attribute value.</param>
    /// <returns><see langword="true"/> if the attribute is successfully found and removed.</returns>
    /// <exception cref="ArgumentNullException" />
    public bool Remove(string name, [NotNullWhen(true)] out IHtmlContent? encodedValue)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (_attributes.TryGetAttribute(name, out var attribute))
        {
            encodedValue = new HtmlString(GetEncodedAttributeValue(attribute));
            _attributes.Remove(attribute);
            return true;
        }
        else
        {
            encodedValue = default;
            return false;
        }
    }

    /// <summary>
    /// Sets an attribute with a value.
    /// </summary>
    /// <remarks>
    /// If an attribute already exists with the specified <paramref name="name"/> then it will be overwritten.
    /// Otherwise, the attribute is added.
    /// </remarks>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    /// <exception cref="ArgumentNullException" />
    public void Set(string name, IHtmlContent value)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(value);

        _attributes.SetAttribute(new TagHelperAttribute(name, value));
    }

    /// <summary>
    /// Sets an attribute with a value.
    /// </summary>
    /// <remarks>
    /// If an attribute already exists with the specified <paramref name="name"/> then it will be overwritten.
    /// Otherwise, the attribute is added.
    /// </remarks>
    /// <param name="name">The attribute name.</param>
    /// <param name="value">The attribute value.</param>
    /// <param name="encodeValue">Whether the value should be HTML encoded.</param>
    /// <exception cref="ArgumentNullException" />
    public void Set(string name, string value, bool encodeValue)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(value);

        _attributes.SetAttribute(CreateAttribute(name, value, encodeValue));
    }

    private static TagHelperAttribute CreateAttribute(string name, string value, bool encodeValue = true) =>
        new TagHelperAttribute(name, encodeValue ? value : new HtmlString(value));

    private static string GetEncodedAttributeValue(TagHelperAttribute attribute) =>
        attribute.ValueStyle == HtmlAttributeValueStyle.Minimized ?
            BooleanAttributeValue :
            attribute.Value switch
            {
                IHtmlContent htmlContent => htmlContent.ToHtmlString(),
                string str => str,
                var value => value.ToString()
            } ?? string.Empty;

    internal ReadOnlyTagHelperAttributeList AsTagHelperAttributes() => _attributes;

    bool IReadOnlyDictionary<string, string>.ContainsKey(string key) => _attributes.ContainsName(key);

    bool IReadOnlyDictionary<string, string>.TryGetValue(string key, [MaybeNullWhen(false)] out string value)
    {
        if (_attributes.TryGetAttribute(key, out var attr))
        {
            value = GetEncodedAttributeValue(attr);
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator() =>
        _attributes.Select(a => KeyValuePair.Create(a.Name, GetEncodedAttributeValue(a))).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyDictionary<string, string>)this).GetEnumerator();
}
