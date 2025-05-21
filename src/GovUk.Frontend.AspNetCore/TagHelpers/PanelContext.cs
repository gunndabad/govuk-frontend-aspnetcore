using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class PanelContext
{
    public IHtmlContent? Body { get; private set; }
    public IHtmlContent? Title { get; private set; }

    public void SetBody(IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        if (Body != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(PanelBodyTagHelper.TagName, PanelTagHelper.TagName);
        }

        Body = content;
    }

    public void SetTitle(IHtmlContent content)
    {
        Guard.ArgumentNotNull(nameof(content), content);

        if (Title != null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(PanelTitleTagHelper.TagName, PanelTagHelper.TagName);
        }

        if (Body != null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(PanelTitleTagHelper.TagName, PanelBodyTagHelper.TagName);
        }

        Title = content;
    }

    public void ThrowIfNotComplete()
    {
        if (Title == null)
        {
            throw ExceptionHelper.AChildElementMustBeProvided(PanelTitleTagHelper.TagName);
        }
    }
}
