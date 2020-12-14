#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class AccordionItemContext
    {
        public (IDictionary<string, string> attributes, IHtmlContent content)? Heading { get; private set; }
        public (IDictionary<string, string> attributes, IHtmlContent content)? Summary { get; private set; }

        public void SetHeading(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

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
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Summary != null)
            {
                throw new InvalidOperationException(
                    $"Only one <{AccordionItemSummaryTagHelper.TagName}> is permitted for each <{AccordionItemTagHelper.TagName}>.");
            }

            Summary = (attributes, content);
        }
    }
}
