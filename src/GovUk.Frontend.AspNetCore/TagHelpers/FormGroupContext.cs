using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal abstract class FormGroupContext
{
    public (string? VisuallyHiddenText, AttributeDictionary? Attributes, IHtmlContent? Content)? ErrorMessage { get; private set; }

    public (AttributeDictionary? Attributes, IHtmlContent? Content)? Hint { get; private set; }

    public (bool IsPageHeading, AttributeDictionary? Attributes, IHtmlContent? Content)? Label { get; private set; }

    protected abstract string ErrorMessageTagName { get; }

    protected abstract string HintTagName { get; }

    protected abstract string LabelTagName { get; }

    protected abstract string RootTagName { get; }

    public virtual void SetErrorMessage(
        string? visuallyHiddenText,
        AttributeDictionary? attributes,
        IHtmlContent? content)
    {
        if (ErrorMessage != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(ErrorMessageTagName, RootTagName);
        }

        ErrorMessage = (visuallyHiddenText, attributes, content);
    }

    public virtual void SetHint(AttributeDictionary? attributes, IHtmlContent? content)
    {
        if (Hint != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(HintTagName, RootTagName);
        }

        if (ErrorMessage != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(HintTagName, ErrorMessageTagName);
        }

        Hint = (attributes, content);
    }

    public virtual void SetLabel(bool isPageHeading, AttributeDictionary? attributes, IHtmlContent? content)
    {
        if (Label != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(LabelTagName!, RootTagName);
        }

        if (Hint != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(LabelTagName, HintTagName);
        }

        if (ErrorMessage != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(LabelTagName, ErrorMessageTagName);
        }

        Label = (isPageHeading, attributes, content);
    }
}
