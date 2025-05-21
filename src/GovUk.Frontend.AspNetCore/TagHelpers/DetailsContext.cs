using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class DetailsContext
{
    public (AttributeDictionary Attributes, IHtmlContent Content)? Summary { get; private set; }

    public (AttributeDictionary Attributes, IHtmlContent Content)? Text { get; private set; }

    public void SetSummary(AttributeDictionary attributes, IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(attributes), attributes);
        Guard.ArgumentNotNull(nameof(content), content);

        if (Summary != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(DetailsSummaryTagHelper.TagName, DetailsTagHelper.TagName);
        }

        if (Text != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(DetailsSummaryTagHelper.TagName, DetailsTextTagHelper.TagName);
        }

        Summary = (attributes, content);
    }

    public void SetText(AttributeDictionary attributes, IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(attributes), attributes);
        Guard.ArgumentNotNull(nameof(content), content);

        if (Text != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(DetailsTextTagHelper.TagName, DetailsTagHelper.TagName);
        }

        Text = (attributes, content);
    }

    public void ThrowIfNotComplete()
    {
        if (Summary == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(DetailsSummaryTagHelper.TagName);
        }

        if (Text == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(DetailsTextTagHelper.TagName);
        }
    }
}
