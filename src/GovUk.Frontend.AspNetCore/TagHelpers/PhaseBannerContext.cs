#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class PhaseBannerContext
    {
        public (IDictionary<string, string>? Attributes, IHtmlContent Content)? Tag { get; private set; }

        public void SetTag(IDictionary<string, string>? attributes, IHtmlContent content)
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
}
