using GovUk.Frontend.AspNetCore.TestCommon;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData("hint", typeof(OptionsJson.Hint))]
        public void Hint(ComponentTestCaseData<OptionsJson.Hint> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html) ?? _emptyContent;

                    var attributes = options.Attributes.ToAttributesDictionary()
                        .MergeAttribute("class", options.Classes);

                    return generator.GenerateHint(options.Id, content, attributes).RenderToString();
                });
    }
}
