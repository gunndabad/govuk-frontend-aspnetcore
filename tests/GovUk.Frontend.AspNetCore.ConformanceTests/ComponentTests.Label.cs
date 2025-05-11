using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("label", typeof(OptionsJson.Label), exclude: "empty")]
    public void Label(ComponentTestCaseData<OptionsJson.Label> data)
    {
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var labelOptions = options with { For = options.For };

                return BuildLabel(generator, labelOptions).ToHtmlString();
            });
    }

    private static IHtmlContent BuildLabel(ComponentGenerator generator, OptionsJson.Label options)
    {
        var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);

        var attributes = options.Attributes.ToAttributesDictionary()
            .MergeAttribute("class", options.Classes);

        return generator.GenerateLabel(
            options.For,
            options.IsPageHeading ?? ComponentGenerator.LabelDefaultIsPageHeading,
            content,
            attributes);
    }
}
