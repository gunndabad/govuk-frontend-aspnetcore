using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class NotificationBannerContext
{
    public (string Id, int HeadingLevel, IHtmlContent? Content)? Title { get; private set; }

    public void SetTitle(string id, int headingLevel, IHtmlContent? content)
    {
        Guard.ArgumentNotNullOrEmpty(nameof(id), id);

        if (Title != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                NotificationBannerTitleTagHelper.TagName,
                NotificationBannerTagHelper.TagName);
        }

        Title = (id, headingLevel, content);
    }
}
