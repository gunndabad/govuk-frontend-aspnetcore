using System.Diagnostics;
using System.Text.Encodings.Web;
using Fluid.Values;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

/// <summary>
/// Contains either an unencoded <see cref="System.String" /> or an <see cref="IHtmlContent"/>.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public sealed class TemplateString
{
    private static readonly HtmlEncoder _defaultEncoder = HtmlEncoder.Default;

    private readonly object? _value;

    /// <summary>
    /// Creates a new <see cref="TemplateString"/> from an unencoded <see cref="String"/>.
    /// </summary>
    /// <param name="value">The unencoded <see cref="String"/>.</param>
    public TemplateString(string? value)
    {
        _value = value ?? string.Empty;
    }

    /// <summary>
    /// Creates a new <see cref="TemplateString"/> from an <see cref="IHtmlContent"/>.
    /// </summary>
    /// <param name="content">The <see cref="IHtmlContent"/>.</param>
    public TemplateString(IHtmlContent? content)
    {
        _value = (object?)content ?? string.Empty;
    }

    /// <summary>
    /// Gets an encoded HTML string for the current <see cref="TemplateString"/>.
    /// </summary>
    /// <param name="encoder">The <see cref="HtmlEncoder"/> to encoded unencoded values with.</param>
    /// <returns>The encoded HTML.</returns>
    public string ToHtmlString(HtmlEncoder encoder)
    {
        ArgumentNullException.ThrowIfNull(encoder);

        if (_value is string str)
        {
            return encoder.Encode(str);
        }
        else
        {
            Debug.Assert(_value is IHtmlContent);
            return ((IHtmlContent)_value).ToHtmlString(encoder);
        }
    }

    internal FluidValue ToFluidValue(HtmlEncoder encoder)
    {
        if (_value is null)
        {
            return NilValue.Instance;
        }

        if (_value is string str)
        {
            return new StringValue(str, encode: true);
        }

        Debug.Assert(_value is IHtmlContent);
        var html = ((IHtmlContent)_value).ToHtmlString();
        return new StringValue(html, encode: false);
    }

    /// <summary>
    /// A <see cref="TemplateString"/> with no content.
    /// </summary>
    public static TemplateString Empty { get; } = new((string?)null);

    /// <summary>
    /// Creates a new <see cref="TemplateString"/> from the specified unencoded <see cref="String"/>.
    /// </summary>
    /// <param name="value">The unencoded <see cref="String"/>.</param>
    /// <returns></returns>
    public static implicit operator TemplateString(string? value) => new(value);

    /// <summary>
    /// Creates a <see cref="TemplateString"/> from <see cref="HtmlString"/>.
    /// </summary>
    /// <param name="content">The <see cref="IHtmlContent"/> to create the <see cref="TemplateString"/> from.</param>
    /// <returns>A new <see cref="TemplateString"/> wrapping the specified <see cref="HtmlString"/>.</returns>
    public static implicit operator TemplateString(HtmlString? content) => new(content);

    /// <inheritdoc/>
    public override string ToString() => ToHtmlString(_defaultEncoder);

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not TemplateString otherTemplateString)
        {
            return false;
        }

        return otherTemplateString.ToHtmlString(_defaultEncoder).Equals(ToHtmlString(_defaultEncoder));
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return ToHtmlString(_defaultEncoder).GetHashCode();
    }
}

/// <summary>
/// Extensions for <see cref="TemplateString"/>.
/// </summary>
public static class TemplateStringExtensions
{
    /// <summary>
    /// Creates a <see cref="TemplateString"/> from <see cref="IHtmlContent"/>.
    /// </summary>
    /// <param name="content">The <see cref="IHtmlContent"/> to create the <see cref="TemplateString"/> from.</param>
    /// <returns>A new <see cref="TemplateString"/> wrapping the specified <see cref="IHtmlContent"/>.</returns>
    public static TemplateString ToTemplateString(this IHtmlContent? content) => new(content);
}
