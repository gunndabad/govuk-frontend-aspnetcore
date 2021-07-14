#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class AccordionItemContext
    {
        public (IDictionary<string, string> Attributes, IHtmlContent Content)? Heading { get; private set; }
        public (IDictionary<string, string> Attributes, IHtmlContent Content)? Summary { get; private set; }

        public void SetHeading(IDictionary<string, string> attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(attributes), attributes);
            Guard.ArgumentNotNull(nameof(content), content);

            if (Heading != null)
            {
                throw new InvalidOperationException(
                    $"Only one <{AccordionItemHeadingTagHelper.TagName}> is permitted for each <{AccordionItemTagHelper.TagName}>.");
            }

            if (Summary != null)
            {
                throw new InvalidOperationException(
                    $"<{AccordionItemHeadingTagHelper.TagName}> must be specified before <{AccordionItemSummaryTagHelper.TagName}>.");
            }

            Heading = (attributes, content);
        }

        public void SetSummary(IDictionary<string, string> attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(attributes), attributes);
            Guard.ArgumentNotNull(nameof(content), content);

            if (Summary != null)
            {
                throw new InvalidOperationException(
                    $"Only one <{AccordionItemSummaryTagHelper.TagName}> is permitted for each <{AccordionItemTagHelper.TagName}>.");
            }

            Summary = (attributes, content);
        }

        public void ThrowIfIncomplete()
        {
            if (Heading == null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(AccordionItemHeadingTagHelper.TagName);
            }
        }
    }
}
