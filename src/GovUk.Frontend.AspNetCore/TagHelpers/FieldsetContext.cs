#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class FieldsetContext
    {
        public (bool IsPageHeading, IDictionary<string, string>? Attributes, IHtmlContent Content)? Legend { get; private set; }

        public void SetLegend(
            bool isPageHeading,
            IDictionary<string, string>? attributes,
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
    }
}
