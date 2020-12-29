using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
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
                (generator, options) => BuildHint(generator, options).RenderToString());

        private static IHtmlContent BuildHint(ComponentGenerator generator, OptionsJson.Hint options)
        {
            var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html) ?? _emptyContent;

            var attributes = options.Attributes.ToAttributesDictionary()
                .MergeAttribute("class", options.Classes);

            return generator.GenerateHint(options.Id, content, attributes);
        }
    }
}
