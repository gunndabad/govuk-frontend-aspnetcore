using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class RazorGenerator
    {
        public string GenerateAccordion(Accordion options)
        {
            var accordion = new TagBuilder("govuk-accordion");
            accordion.Attributes.Add("id", options.Id);

            if (options.HeadingLevel.HasValue)
            {
                accordion.Attributes.Add("heading-level", options.HeadingLevel.Value.ToString());
            }

            accordion.AddCssClass(options.Classes);
            accordion.MergeAttributes(options.Attributes);

            foreach (var item in options.Items)
            {
                var accordionItem = new TagBuilder("govuk-accordion-item");

                if (item.Expanded.HasValue)
                {
                    accordionItem.Attributes.Add("expanded", item.Expanded.Value ? "true" : "false");
                }

                var accordionItemHeading = new TagBuilder("govuk-accordion-item-heading");
                accordionItemHeading.InnerHtml.AppendHtml(item.Heading.GetHtmlContent());
                accordionItem.InnerHtml.AppendHtml(accordionItemHeading);

                if (item.Summary != null)
                {
                    var accordionItemSummary = new TagBuilder("govuk-accordion-item-summary");
                    accordionItemSummary.InnerHtml.AppendHtml(item.Summary.GetHtmlContent());
                    accordionItem.InnerHtml.AppendHtml(accordionItemSummary);
                }

                accordionItem.InnerHtml.AppendHtml(item.Content.GetHtmlContent());

                accordion.InnerHtml.AppendHtml(accordionItem);
            }

            return accordion.RenderToString();
        }
    }
}
