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
            accordion.Attributes.Add("id", options.Id ?? "GFA_test");

            if (options.HeadingLevel.HasValue)
            {
                accordion.Attributes.Add("heading-level", options.HeadingLevel.Value.ToString());
            }

            accordion.AddAttributes(options.Attributes);
            accordion.AddCssClass(options.Classes);

            foreach (var item in options.Items)
            {
                var accordionItem = new TagBuilder("govuk-accordion-item");

                if (item.Expanded.HasValue)
                {
                    accordionItem.AddAttribute("expanded", item.Expanded.Value);
                }

                var accordionItemHeading = new TagBuilder("govuk-accordion-item-heading");
                accordionItemHeading.InnerHtml.AppendHtml(TextOrHtmlHelper.GetHtmlContent(item.Heading.Text, item.Heading.Html));
                accordionItem.InnerHtml.AppendHtml(accordionItemHeading);

                if (item.Summary != null)
                {
                    var accordionItemSummary = new TagBuilder("govuk-accordion-item-summary");
                    accordionItemSummary.InnerHtml.AppendHtml(TextOrHtmlHelper.GetHtmlContent(item.Summary.Text, item.Summary.Html));
                    accordionItem.InnerHtml.AppendHtml(accordionItemSummary);
                }

                accordionItem.InnerHtml.AppendHtml(TextOrHtmlHelper.GetHtmlContent(item.Content.Text, item.Content.Html));

                accordion.InnerHtml.AppendHtml(accordionItem);
            }

            return accordion.RenderToString();
        }
    }
}
