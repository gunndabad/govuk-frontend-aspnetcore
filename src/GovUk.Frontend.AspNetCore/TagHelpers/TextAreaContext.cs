using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class TextAreaContext : FormGroupContext
{
    protected override string ErrorMessageTagName => TextAreaTagHelper.ErrorMessageTagName;

    protected override string HintTagName => TextAreaTagHelper.HintTagName;

    protected override string LabelTagName => TextAreaTagHelper.LabelTagName;

    protected override string RootTagName => TextAreaTagHelper.TagName;

    public IHtmlContent? Value { get; private set; }

    public override void SetErrorMessage(string? visuallyHiddenText, AttributeDictionary? attributes, IHtmlContent? content)
    {
        if (Value != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                ErrorMessageTagName,
                TextAreaValueTagHelper.TagName);
        }

        base.SetErrorMessage(visuallyHiddenText, attributes, content);
    }

    public override void SetHint(AttributeDictionary? attributes, IHtmlContent? content)
    {
        if (Value != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                HintTagName,
                TextAreaValueTagHelper.TagName);
        }

        base.SetHint(attributes, content);
    }

    public override void SetLabel(bool isPageHeading, AttributeDictionary? attributes, IHtmlContent? content)
    {
        if (Value != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                LabelTagName,
                TextAreaValueTagHelper.TagName);
        }

        base.SetLabel(isPageHeading, attributes, content);
    }

    public void SetValue(IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        if (Value != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(TextAreaValueTagHelper.TagName, RootTagName);
        }

        Value = content;
    }
}
