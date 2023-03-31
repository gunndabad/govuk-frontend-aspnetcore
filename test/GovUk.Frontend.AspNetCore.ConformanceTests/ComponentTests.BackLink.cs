using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData("back-link", typeof(OptionsJson.BackLink))]
        public void BackLink(ComponentTestCaseData<OptionsJson.BackLink> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var attributes = options.Attributes.ToAttributesDictionary()
                        .MergeAttribute("class", options.Classes);

                    if (!attributes.ContainsKey("href"))
                    {
                        attributes.Add("href", "#");
                    }

                    var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html) ??
                        new HtmlString(ComponentGenerator.BackLinkDefaultContent);

                    return generator.GenerateBackLink(content, attributes)
                        .ToHtmlString();
                });
    }
}
