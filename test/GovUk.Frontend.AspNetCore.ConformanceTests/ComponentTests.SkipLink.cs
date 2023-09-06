using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("skip-link", typeof(OptionsJson.SkipLink))]
    public void SkipLink(ComponentTestCaseData<OptionsJson.SkipLink> data) =>
           CheckComponentHtmlMatchesExpectedHtml(
               data,
               (generator, options) =>
               {
                   var href = options.Href ?? ComponentGenerator.SkipLinkDefaultHref;

                   var attributes = options.Attributes.ToAttributesDictionary()
                       .MergeAttribute("class", options.Classes);

                   var content = TextOrHtmlHelper.GetHtmlContent(options.Text, options.Html) ?? new HtmlString("");

                   return generator.GenerateSkipLink(href, content, attributes)
                       .ToHtmlString();
               });
}
