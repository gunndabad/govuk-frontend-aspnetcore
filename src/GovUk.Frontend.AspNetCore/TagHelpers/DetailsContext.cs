#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class DetailsContext
    {
        public (IDictionary<string, string> Attributes, IHtmlContent Content)? Summary { get; private set; }

        public (IDictionary<string, string> Attributes, IHtmlContent Content)? Text { get; private set; }

        public void SetSummary(IDictionary<string, string> attributes, IHtmlContent content)
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

        public void SetText(IDictionary<string, string> attributes, IHtmlContent content)
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
}
