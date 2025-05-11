using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class CharacterCountContext : FormGroupContext2
{
    protected override IReadOnlyCollection<string> ErrorMessageTagNames => [CharacterCountTagHelper.ErrorMessageTagName];

    protected override IReadOnlyCollection<string> HintTagNames => [CharacterCountTagHelper.HintTagName];

    protected override IReadOnlyCollection<string> LabelTagNames => [CharacterCountTagHelper.LabelTagName];

    protected override string RootTagName => CharacterCountTagHelper.TagName;

    public IHtmlContent? Value { get; private set; }

    public override void SetErrorMessage(
        IHtmlContent? visuallyHiddenText,
        EncodedAttributesDictionary attributes,
        IHtmlContent? html,
        string tagName)
    {
        if (Value != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                CharacterCountValueTagHelper.TagName);
        }

        base.SetErrorMessage(visuallyHiddenText, attributes, html, tagName);
    }

    public override void SetHint(EncodedAttributesDictionary attributes, IHtmlContent? html, string tagName)
    {
        if (Value != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                CharacterCountValueTagHelper.TagName);
        }

        base.SetHint(attributes, html, tagName);
    }

    public override void SetLabel(
        bool isPageHeading,
        EncodedAttributesDictionary attributes,
        IHtmlContent? html,
        string tagName)
    {
        if (Value != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                CharacterCountValueTagHelper.TagName);
        }

        base.SetLabel(isPageHeading, attributes, html, tagName);
    }

    public void SetValue(IHtmlContent html, string tagName)
    {
        if (Value != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(tagName, RootTagName);
        }

        Value = html;
    }
}
