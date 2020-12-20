using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class RazorGenerator
    {
        public string GenerateDetails(Details options)
        {
            var details = new TagBuilder("govuk-details");

            details.AddAttributes(options.Attributes);
            details.AddCssClass(options.Classes);

            if (options.Id != null)
            {
                details.Attributes.Add("id", options.Id);
            }

            if (options.Open.HasValue)
            {
                details.Attributes.Add("open", options.Open.Value ? "true" : "false");
            }

            var detailsSummary = new TagBuilder("govuk-details-summary");
            detailsSummary.InnerHtml.AppendHtml(
                TextOrHtmlHelper.GetHtmlContent(options.SummaryText, options.SummaryHtml));
            details.InnerHtml.AppendHtml(detailsSummary);

            var detailsText = new TagBuilder("govuk-details-text");
            detailsText.InnerHtml.AppendHtml(TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html));
            details.InnerHtml.AppendHtml(detailsText);

            return details.RenderToString();
        }
    }
}
