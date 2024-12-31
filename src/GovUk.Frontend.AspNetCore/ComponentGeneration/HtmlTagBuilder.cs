using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// A type for building HTML tags.
/// </summary>
public class HtmlTagBuilder : IHtmlContent
{
    private static readonly HashSet<string> _voidElementTags = new(StringComparer.OrdinalIgnoreCase)
    {
        "area",
        "base",
        "br",
        "col",
        "embed",
        "hr",
        "img",
        "input",
        "link",
        "meta",
        "param",
        "source",
        "track",
        "wbr",

        "path"
    };

    private readonly HtmlContentBuilder _innerContent;
    private readonly EncodedAttributesDictionary _attributes;
    private readonly bool _isVoidElement;

    /// <summary>
    /// Creates a new <see cref="HtmlTagBuilder"/>.
    /// </summary>
    /// <param name="tagName">The tag name for the node.</param>
    public HtmlTagBuilder(string tagName)
    {
        ArgumentNullException.ThrowIfNull(tagName);

        TagName = tagName;
        _innerContent = new HtmlContentBuilder();
        _attributes = new();
        _isVoidElement = _voidElementTags.Contains(tagName);
    }

    /// <summary>
    /// Gets the tag's name.
    /// </summary>
    public string TagName { get; }

    /// <summary>
    /// Gets the tag's inner content.
    /// </summary>
    public IHtmlContent InnerContent => _innerContent;

    /// <inheritdoc cref="EncodedAttributesDictionary.AddBoolean(string)"/>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder WithBooleanAttribute(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        _attributes.AddBoolean(name);

        return this;
    }

    /// <inheritdoc cref="EncodedAttributesDictionary.Add(EncodedAttributesDictionary)"/>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder WithAttributes(EncodedAttributesDictionary? other)
    {
        if (other is not null)
        {
            _attributes.Add(other);
        }

        return this;
    }

    /// <inheritdoc cref="EncodedAttributesDictionary.Add(string, string, bool)"/>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder WithAttribute(string name, IHtmlContent value)
    {
        ArgumentNullException.ThrowIfNull(name);

        _attributes.Add(name, value);

        return this;
    }

    /// <inheritdoc cref="EncodedAttributesDictionary.Add(string, string, bool)"/>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder WithAttribute(string name, string value, bool encodeValue)
    {
        ArgumentNullException.ThrowIfNull(name);

        _attributes.Add(name, value, encodeValue);

        return this;
    }

    /// <summary>
    /// Appends content to this tag's inner content.
    /// </summary>
    /// <param name="content">The content to append.</param>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder WithAppendedHtml(IHtmlContent content)
    {
        ArgumentNullException.ThrowIfNull(content);

        ThrowOnAppendIfVoidElement();

        _innerContent.AppendHtml(content);

        return this;
    }

    /// <summary>
    /// Appends content to this tag's inner content.
    /// </summary>
    /// <param name="getContent">A delegate that gets the content to append.</param>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder WithAppendedHtml(Func<IHtmlContent> getContent)
    {
        ArgumentNullException.ThrowIfNull(getContent);

        ThrowOnAppendIfVoidElement();

        _innerContent.AppendHtml(getContent());

        return this;
    }

    /// <summary>
    /// Appends pre-encoded HTML to this tag's inner content.
    /// </summary>
    /// <param name="encoded">The encoded HTML to append.</param>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder WithAppendedHtml(string encoded)
    {
        ArgumentNullException.ThrowIfNull(encoded);

        ThrowOnAppendIfVoidElement();

        _innerContent.AppendHtml(encoded);

        return this;
    }

    /// <summary>
    /// Appends text to this tag's inner content.
    /// </summary>
    /// <remarks>
    /// The text will be HTML encoded.
    /// </remarks>
    /// <param name="text">The text to append.</param>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder WithAppendedText(string text)
    {
        ThrowOnAppendIfVoidElement();

        _innerContent.Append(text);

        return this;
    }

    /// <inheritdoc cref="EncodedAttributesDictionary.AddCssClass(string)"/>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder WithCssClass(string className)
    {
        ArgumentNullException.ThrowIfNull(className);

        _attributes.AddCssClass(className);

        return this;
    }

    /// <inheritdoc cref="EncodedAttributesDictionary.AddCssClasses(IEnumerable{string})"/>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder WithCssClasses(IEnumerable<string> classNames)
    {
        ArgumentNullException.ThrowIfNull(classNames);

        _attributes.AddCssClasses(classNames);

        return this;
    }

    /// <summary>
    /// Writes the tag to the specified <see cref="TagHelperOutput"/>.
    /// </summary>
    /// <param name="output">The <see cref="TagHelperOutput"/> to write the tag to.</param>
    public virtual void WriteTo(TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(output);

        output.TagMode = _isVoidElement ? TagMode.StartTagOnly : TagMode.StartTagAndEndTag;
        output.TagName = TagName;

        output.Attributes.Clear();

        foreach (var attribute in _attributes.AsTagHelperAttributes())
        {
            output.Attributes.Add(attribute);
        }

        output.Content.AppendHtml(InnerContent);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        using var writer = new StringWriter();
        ((IHtmlContent)this).WriteTo(writer, HtmlEncoder.Default);
        return writer.ToString();
    }

    internal HtmlTagBuilder When(Func<bool> condition, Action<HtmlTagBuilder> action)
    {
        if (condition())
        {
            action(this);
        }

        return this;
    }

    internal HtmlTagBuilder When(bool condition, Action<HtmlTagBuilder> action) =>
        When(() => condition, action);

    internal HtmlTagBuilder WhenNotNull<T>(Func<T?> getValue, Action<T, HtmlTagBuilder> action)
    {
        var value = getValue();
        if (value is not null)
        {
            action(value, this);
        }

        return this;
    }

    internal HtmlTagBuilder WhenNotNull<T>(T? value, Action<T, HtmlTagBuilder> action) =>
        WhenNotNull(() => value, action);

    internal HtmlTagBuilder WithAttributeWhenNotNull(Func<string?> getValue, string name, bool encodeValue) =>
        WhenNotNull(getValue(), (value, b) => b.WithAttribute(name, value, encodeValue));

    internal HtmlTagBuilder WithAttributeWhenNotNull(string? value, string name, bool encodeValue) =>
        WhenNotNull(value, (_, b) => b.WithAttribute(name, value!, encodeValue));

    internal HtmlTagBuilder WithAttributeWhenNotNull(Func<IHtmlContent?> getValue, string name) =>
        WhenNotNull(getValue(), (value, b) => b.WithAttribute(name, value));

    internal HtmlTagBuilder WithAttributeWhenNotNull(IHtmlContent? value, string name) =>
        WhenNotNull(value, (_, b) => b.WithAttribute(name, value!));

    void IHtmlContent.WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write("<");
        writer.Write(TagName);
        foreach (var attribute in _attributes.AsTagHelperAttributes())
        {
            writer.Write(" ");
            attribute.WriteTo(writer, encoder);
        }
        writer.Write(">");

        if (_isVoidElement)
        {
            return;
        }

        _innerContent.WriteTo(writer, encoder);

        writer.Write("</");
        writer.Write(TagName);
        writer.Write(">");
    }

    private void ThrowOnAppendIfVoidElement()
    {
        if (_isVoidElement)
        {
            throw new InvalidOperationException("Void elements can have not children appended.");
        }
    }
}
