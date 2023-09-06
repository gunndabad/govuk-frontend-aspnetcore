using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class PhaseBannerContext
{
    public (AttributeDictionary? Attributes, IHtmlContent Content)? Tag { get; private set; }

    public void SetTag(AttributeDictionary? attributes, IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(attributes), attributes);
        Guard.ArgumentNotNull(nameof(content), content);

        if (Tag != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                PhaseBannerTagTagHelper.TagName, PhaseBannerTagHelper.TagName);
        }

        Tag = (attributes, content);
    }

    public void ThrowIfIncomplete()
    {
        if (Tag == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(PhaseBannerTagTagHelper.TagName);
        }
    }
}
