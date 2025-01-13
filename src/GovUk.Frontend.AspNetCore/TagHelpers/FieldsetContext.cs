using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class FieldsetContext
{
    public (bool IsPageHeading, AttributeDictionary? Attributes, IHtmlContent Content)? Legend { get; private set; }

    public void SetLegend(
        bool isPageHeading,
        AttributeDictionary? attributes,
        IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        if (Legend != null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                FieldsetLegendTagHelper.TagName,
                FieldsetTagHelper.TagName);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        Legend = (isPageHeading, attributes, content);
    }

    public void ThrowIfNotComplete()
    {
        if (Legend == null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            throw ExceptionHelper.AChildElementMustBeProvided(FieldsetLegendTagHelper.TagName);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
