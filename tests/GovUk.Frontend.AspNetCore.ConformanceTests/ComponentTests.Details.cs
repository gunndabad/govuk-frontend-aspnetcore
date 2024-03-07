using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("details", typeof(OptionsJson.Details))]
    public void Details(ComponentTestCaseData<OptionsJson.Details> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var open = options.Open ?? ComponentGenerator.DetailsDefaultOpen;

                var summaryContent = TextOrHtmlHelper.GetHtmlContent(options.SummaryText, options.SummaryHtml);

                var textContent = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);

                var attributes = options.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Classes)
                    .MergeAttribute("id", options.Id);

                return generator.GenerateDetails(
                        open,
                        summaryContent,
                        summaryAttributes: null,
                        textContent,
                        textAttributes: null,
                        attributes)
                    .ToHtmlString();
            });
}
