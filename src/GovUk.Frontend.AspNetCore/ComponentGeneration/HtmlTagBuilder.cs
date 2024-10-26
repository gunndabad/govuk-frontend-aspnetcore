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
        "hr",
        "img",
        "input",
        "link",
        "meta",
        "param",
        "command",
        "keygen",
        "source",
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
    public HtmlTagBuilder AddBooleanAttribute(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        _attributes.AddBoolean(name);

        return this;
    }

    /// <inheritdoc cref="EncodedAttributesDictionary.Add(EncodedAttributesDictionary)"/>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder AddAttributes(EncodedAttributesDictionary other)
    {
        ArgumentNullException.ThrowIfNull(other);

        _attributes.Add(other);

        return this;
    }

    /// <inheritdoc cref="EncodedAttributesDictionary.Add(string, string, bool)"/>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder AddAttribute(string name, string value, bool encodeValue)
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
    public HtmlTagBuilder AppendHtml(IHtmlContent content)
    {
        ThrowOnAppendIfVoidElement();

        _innerContent.AppendHtml(content);

        return this;
    }

    /// <summary>
    /// Appends pre-encoded HTML to this tag's inner content.
    /// </summary>
    /// <param name="encoded">The encoded HTML to append.</param>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder AppendHtml(string encoded)
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
    public HtmlTagBuilder AppendText(string text)
    {
        ThrowOnAppendIfVoidElement();

        _innerContent.Append(text);

        return this;
    }

    /// <inheritdoc cref="EncodedAttributesDictionary.AddCssClass(string)"/>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder AddCssClass(string className)
    {
        ArgumentNullException.ThrowIfNull(className);

        _attributes.AddCssClass(className);

        return this;
    }

    /// <inheritdoc cref="EncodedAttributesDictionary.AddCssClasses(IEnumerable{string})"/>
    /// <returns>This <see cref="HtmlTagBuilder"/> to allow calls to be chained.</returns>
    public HtmlTagBuilder AddCssClasses(IEnumerable<string> classNames)
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
