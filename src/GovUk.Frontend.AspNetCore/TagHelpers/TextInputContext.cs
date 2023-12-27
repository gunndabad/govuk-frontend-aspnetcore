using System;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.ComponentGeneration;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class TextInputContext : FormGroupContext2
{
    internal record PrefixSuffixInfo(ImmutableDictionary<string, string?> Attributes, string Html);

    // internal for testing
    internal PrefixSuffixInfo? Prefix;
    internal PrefixSuffixInfo? Suffix;

    protected override string ErrorMessageTagName => TextInputTagHelper.ErrorMessageTagName;

    protected override string HintTagName => TextInputTagHelper.HintTagName;

    protected override string LabelTagName => TextInputTagHelper.LabelTagName;

    protected override string RootTagName => TextInputTagHelper.TagName;

    public TextInputOptionsPrefix? GetPrefixOptions() => Prefix is not null ?
        new TextInputOptionsPrefix()
        {
            Text = null,
            Html = Prefix.Html,
            Attributes = Prefix.Attributes.Remove("class", out var classes),
            Classes = classes
        } :
        null;

    public TextInputOptionsSuffix? GetSuffixOptions() => Suffix is not null ?
        new TextInputOptionsSuffix()
        {
            Text = null,
            Html = Suffix.Html,
            Attributes = Suffix.Attributes.Remove("class", out var classes),
            Classes = classes
        } :
        null;

    public override void SetErrorMessage(
        string? visuallyHiddenText,
        ImmutableDictionary<string, string?> attributes,
        string? html)
    {
        if (Prefix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                ErrorMessageTagName,
                TextInputPrefixTagHelper.TagName);
        }

        if (Suffix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                ErrorMessageTagName,
                TextInputSuffixTagHelper.TagName);
        }

        base.SetErrorMessage(visuallyHiddenText, attributes, html);
    }

    public override void SetHint(ImmutableDictionary<string, string?> attributes, string? html)
    {
        if (Prefix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                HintTagName,
                TextInputPrefixTagHelper.TagName);
        }

        if (Suffix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                HintTagName,
                TextInputSuffixTagHelper.TagName);
        }

        base.SetHint(attributes, html);
    }

    public override void SetLabel(bool isPageHeading, ImmutableDictionary<string, string?> attributes, string? html)
    {
        if (Prefix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                LabelTagName,
                TextInputPrefixTagHelper.TagName);
        }

        if (Suffix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                LabelTagName,
                TextInputSuffixTagHelper.TagName);
        }

        base.SetLabel(isPageHeading, attributes, html);
    }

    public void SetPrefix(ImmutableDictionary<string, string?> attributes, string html)
    {
        ArgumentNullException.ThrowIfNull(html);

        if (Prefix is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                TextInputPrefixTagHelper.TagName,
                TextInputTagHelper.TagName);
        }

        if (Suffix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                TextInputPrefixTagHelper.TagName,
                TextInputSuffixTagHelper.TagName);
        }

        Prefix = new PrefixSuffixInfo(attributes, html);
    }

    public void SetSuffix(ImmutableDictionary<string, string?> attributes, string html)
    {
        ArgumentNullException.ThrowIfNull(html);

        if (Suffix is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                TextInputSuffixTagHelper.TagName,
                TextInputTagHelper.TagName);
        }

        Suffix = new PrefixSuffixInfo(attributes, html);
    }
}
