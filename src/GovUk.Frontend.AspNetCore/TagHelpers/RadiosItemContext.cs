using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class RadiosItemContext
{
    public (AttributeDictionary Attributes, IHtmlContent Content)? Conditional { get; private set; }
    public (AttributeDictionary Attributes, IHtmlContent Content)? Hint { get; private set; }

    public void SetConditional(AttributeDictionary attributes, IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        if (Conditional is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                RadiosItemConditionalTagHelper.TagName,
                RadiosItemTagHelper.TagName);
        }

        Conditional = (attributes, content);
    }

    public void SetHint(AttributeDictionary attributes, IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        if (Hint is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                RadiosItemHintTagHelper.TagName,
                RadiosItemTagHelper.TagName);
        }

        if (Conditional is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                RadiosItemHintTagHelper.TagName,
                RadiosItemConditionalTagHelper.TagName);
        }

        Hint = (attributes, content);
    }
}
