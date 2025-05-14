using GovUk.Frontend.AspNetCore.Components;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class TextInputContext : FormGroupContext3
{
    private (InputOptionsPrefix Options, string TagName)? _prefix;
    private (InputOptionsSuffix Options, string TagName)? _suffix;

    public InputOptionsPrefix? Prefix => _prefix?.Options;

    public InputOptionsSuffix? Suffix => _suffix?.Options;

    protected override IReadOnlyCollection<string> ErrorMessageTagNames => [/*TextInputTagHelper.ErrorMessageShortTagName, */TextInputTagHelper.ErrorMessageTagName];

    protected override IReadOnlyCollection<string> HintTagNames => [/*TextInputTagHelper.HintShortTagName, */TextInputTagHelper.HintTagName];

    protected override IReadOnlyCollection<string> LabelTagNames => [/*TextInputTagHelper.LabelShortTagName, */TextInputTagHelper.LabelTagName];

    private IReadOnlyCollection<string> PrefixTagNames => [/*TextInputPrefixTagHelper.ShortTagName, */TextInputPrefixTagHelper.TagName];

    private IReadOnlyCollection<string> SuffixTagNames => [/*TextInputSuffixTagHelper.ShortTagName, */TextInputSuffixTagHelper.TagName];

    protected override string RootTagName => TextInputTagHelper.TagName;

    public override void SetErrorMessage(
        TemplateString? visuallyHiddenText,
        AttributeCollection attributes,
        TemplateString? html,
        string tagName)
    {
        if (_prefix is var (_, prefixTagName))
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                prefixTagName);
        }

        if (_suffix is var (_, suffixTagName))
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                suffixTagName);
        }

        base.SetErrorMessage(visuallyHiddenText, attributes, html, tagName);
    }

    public override void SetHint(
        AttributeCollection attributes,
        TemplateString? html,
        string tagName)
    {
        if (_prefix is var (_, prefixTagName))
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                prefixTagName);
        }

        if (_suffix is var (_, suffixTagName))
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                suffixTagName);
        }

        base.SetHint(attributes, html, tagName);
    }

    public override void SetLabel(
        bool isPageHeading,
        AttributeCollection attributes,
        TemplateString? html,
        string tagName)
    {
        if (_prefix is var (_, prefixTagName))
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                prefixTagName);
        }

        if (_suffix is var (_, suffixTagName))
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                suffixTagName);
        }

        base.SetLabel(isPageHeading, attributes, html, tagName);
    }

    public void SetPrefix(InputOptionsPrefix options, string tagName)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(tagName);

        if (Prefix is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                PrefixTagNames,
                RootTagName);
        }

        if (_suffix is var (_, suffixTagName))
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                suffixTagName);
        }

        _prefix = (options, tagName);
    }

    public void SetSuffix(InputOptionsSuffix options, string tagName)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(tagName);

        if (Suffix is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                SuffixTagNames,
                RootTagName);
        }

        _suffix = (options, tagName);
    }
}
