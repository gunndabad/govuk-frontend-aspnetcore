using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class CharacterCountContext : FormGroupContext
{
    protected override string ErrorMessageTagName => CharacterCountTagHelper.ErrorMessageTagName;

    protected override string HintTagName => CharacterCountTagHelper.HintTagName;

    protected override string LabelTagName => CharacterCountTagHelper.LabelTagName;

    protected override string RootTagName => CharacterCountTagHelper.TagName;

    public IHtmlContent? Value { get; private set; }

    public override void SetErrorMessage(
        string? visuallyHiddenText,
        AttributeDictionary? attributes,
        IHtmlContent? content
    )
    {
        if (Value != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                ErrorMessageTagName,
                CharacterCountValueTagHelper.TagName
            );
        }

        base.SetErrorMessage(visuallyHiddenText, attributes, content);
    }

    public override void SetHint(AttributeDictionary? attributes, IHtmlContent? content)
    {
        if (Value != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(HintTagName, CharacterCountValueTagHelper.TagName);
        }

        base.SetHint(attributes, content);
    }

    public override void SetLabel(bool isPageHeading, AttributeDictionary? attributes, IHtmlContent? content)
    {
        if (Value != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(LabelTagName, CharacterCountValueTagHelper.TagName);
        }

        base.SetLabel(isPageHeading, attributes, content);
    }

    public void SetValue(IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        if (Value != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(CharacterCountValueTagHelper.TagName, RootTagName);
        }

        Value = content;
    }
}
