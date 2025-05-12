using GovUk.Frontend.AspNetCore.Components;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class CharacterCountContext : FormGroupContext3
{
    protected override IReadOnlyCollection<string> ErrorMessageTagNames => [CharacterCountTagHelper.ErrorMessageTagName];

    protected override IReadOnlyCollection<string> HintTagNames => [CharacterCountTagHelper.HintTagName];

    protected override IReadOnlyCollection<string> LabelTagNames => [CharacterCountTagHelper.LabelTagName];

    protected override string RootTagName => CharacterCountTagHelper.TagName;

    public TemplateString? Value { get; private set; }

    public override void SetErrorMessage(
        TemplateString? visuallyHiddenText,
        AttributeCollection attributes,
        TemplateString? html,
        string tagName)
    {
        if (Value is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                CharacterCountValueTagHelper.TagName);
        }

        base.SetErrorMessage(visuallyHiddenText, attributes, html, tagName);
    }

    public override void SetHint(AttributeCollection attributes, TemplateString? html, string tagName)
    {
        if (Value is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                CharacterCountValueTagHelper.TagName);
        }

        base.SetHint(attributes, html, tagName);
    }

    public override void SetLabel(
        bool isPageHeading,
        AttributeCollection attributes,
        TemplateString? html,
        string tagName)
    {
        if (Value is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                tagName,
                CharacterCountValueTagHelper.TagName);
        }

        base.SetLabel(isPageHeading, attributes, html, tagName);
    }

    public void SetValue(TemplateString html, string tagName)
    {
        if (Value is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(tagName, RootTagName);
        }

        Value = html;
    }
}
