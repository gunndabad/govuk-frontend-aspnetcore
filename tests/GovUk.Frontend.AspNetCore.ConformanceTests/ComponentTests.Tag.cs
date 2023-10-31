using GovUk.Frontend.AspNetCore;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("tag", typeof(OptionsJson.Tag))]
    public void Tag(ComponentTestCaseData<OptionsJson.Tag> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html);

                var attributes = options.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Classes);

                return generator.GenerateTag(content, attributes)
                    .ToHtmlString();
            });
}
