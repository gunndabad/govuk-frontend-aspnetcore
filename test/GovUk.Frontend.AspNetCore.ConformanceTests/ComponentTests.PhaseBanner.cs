using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("phase-banner", typeof(OptionsJson.PhaseBanner))]
    public void PhaseBanner(ComponentTestCaseData<OptionsJson.PhaseBanner> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var tagContent = TextOrHtmlHelper.GetHtmlContent(options.Tag?.Text, options.Tag?.Html) ??
                    _emptyContent;

                var tagAttributes = options.Tag?.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Tag?.Classes);

                var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);

                var attributes = options.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Classes);

                return generator.GeneratePhaseBanner(tagContent, tagAttributes, content, attributes)
                    .ToHtmlString();
            });
}
