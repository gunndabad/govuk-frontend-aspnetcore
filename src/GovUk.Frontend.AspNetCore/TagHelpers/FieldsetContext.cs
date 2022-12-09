using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
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
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    FieldsetLegendTagHelper.TagName,
                    FieldsetTagHelper.TagName);
            }

            Legend = (isPageHeading, attributes, content);
        }

        public void ThrowIfNotComplete()
        {
            if (Legend == null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(FieldsetLegendTagHelper.TagName);
            }
        }
    }
}
