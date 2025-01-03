using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class TextInputContext : FormGroupContext2
{
    internal record PrefixSuffixInfo(EncodedAttributesDictionary Attributes, IHtmlContent Html, string TagName);

    // internal for testing
    internal PrefixSuffixInfo? Prefix;
    internal PrefixSuffixInfo? Suffix;

    protected override IReadOnlyCollection<string> ErrorMessageTagNames => [/*TextInputTagHelper.ErrorMessageShortTagName, */TextInputTagHelper.ErrorMessageTagName];

    protected override IReadOnlyCollection<string> HintTagNames => [/*TextInputTagHelper.HintShortTagName, */TextInputTagHelper.HintTagName];

    protected override IReadOnlyCollection<string> LabelTagNames => [/*TextInputTagHelper.LabelShortTagName, */TextInputTagHelper.LabelTagName];

    private IReadOnlyCollection<string> PrefixTagNames => [/*TextInputPrefixTagHelper.ShortTagName, */TextInputPrefixTagHelper.TagName];

    private IReadOnlyCollection<string> SuffixTagNames => [/*TextInputSuffixTagHelper.ShortTagName, */TextInputSuffixTagHelper.TagName];

    protected override string RootTagName => TextInputTagHelper.TagName;

    public TextInputOptionsPrefix? GetPrefixOptions() => Prefix is not null ?
        new TextInputOptionsPrefix()
        {
            Text = null,
            Html = Prefix.Html,
            Attributes = new EncodedAttributesDictionaryBuilder(Prefix.Attributes).Without("class", out var classes),
            Classes = classes
        } :
        null;

    public TextInputOptionsSuffix? GetSuffixOptions() => Suffix is not null ?
        new TextInputOptionsSuffix()
        {
            Text = null,
            Html = Suffix.Html,
            Attributes = new EncodedAttributesDictionaryBuilder(Suffix.Attributes).Without("class", out var classes),
            Classes = classes
        } :
        null;

    public override void SetErrorMessage(
        IHtmlContent? visuallyHiddenText,
        EncodedAttributesDictionary attributes,
        IHtmlContent? html,
        string tagName)
    {
        if (Prefix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                Prefix.TagName);
        }

        if (Suffix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                Suffix.TagName);
        }

        base.SetErrorMessage(visuallyHiddenText, attributes, html, tagName);
    }

    public override void SetHint(
        EncodedAttributesDictionary attributes,
        IHtmlContent? html,
        string tagName)
    {
        if (Prefix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                Prefix.TagName);
        }

        if (Suffix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                Suffix.TagName);
        }

        base.SetHint(attributes, html, tagName);
    }

    public override void SetLabel(
        bool isPageHeading,
        EncodedAttributesDictionary attributes,
        IHtmlContent? html,
        string tagName)
    {
        if (Prefix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                Prefix.TagName);
        }

        if (Suffix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                Suffix.TagName);
        }

        base.SetLabel(isPageHeading, attributes, html, tagName);
    }

    public void SetPrefix(
        EncodedAttributesDictionary attributes,
        IHtmlContent html,
        string tagName)
    {
        ArgumentNullException.ThrowIfNull(html);

        if (Prefix is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                PrefixTagNames,
                RootTagName);
        }

        if (Suffix is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                Suffix.TagName);
        }

        Prefix = new PrefixSuffixInfo(attributes, html, tagName);
    }

    public void SetSuffix(
        EncodedAttributesDictionary attributes,
        IHtmlContent html,
        string tagName)
    {
        ArgumentNullException.ThrowIfNull(html);

        if (Suffix is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                SuffixTagNames,
                RootTagName);
        }

        Suffix = new PrefixSuffixInfo(attributes, html, tagName);
    }
}
