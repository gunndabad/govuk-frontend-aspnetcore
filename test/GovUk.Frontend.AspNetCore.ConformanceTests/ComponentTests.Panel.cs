using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData(
            "panel",
            typeof(OptionsJson.Panel))]
        public void Panel(ComponentTestCaseData<OptionsJson.Panel> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var headingLevel = options.HeadingLevel.GetValueOrDefault(ComponentGenerator.PanelDefaultHeadingLevel);
                    var titleContent = TextOrHtmlHelper.GetHtmlContent(options.TitleText, options.TitleHtml);
                    var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);

                    var attributes = options.Attributes.ToAttributesDictionary()
                        .MergeAttribute("class", options.Classes);

                    return generator.GeneratePanel(headingLevel, titleContent, content, attributes)
                        .RenderToString();
                });
    }
}
