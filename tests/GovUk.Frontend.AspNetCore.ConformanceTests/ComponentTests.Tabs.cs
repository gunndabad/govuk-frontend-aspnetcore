using System.Linq;
using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData(
        "tabs",
        typeof(OptionsJson.Tabs),
        exclude: new[] { "with falsy values", "empty item list", "no item list" })]
    public void Tabs(ComponentTestCaseData<OptionsJson.Tabs> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var title = options.Title ?? ComponentGenerator.TabsDefaultTitle;

                var attributes = options.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", options.Classes);

                var items = (options.Items?.Select(i => new TabsItem()
                {
                    Id = i.Id,
                    Label = i.Label,
                    LinkAttributes = i.Attributes.ToAttributesDictionary(),
                    PanelAttributes = i.Panel.Attributes.ToAttributesDictionary(),
                    PanelContent = !string.IsNullOrEmpty(i.Panel.Text) ? new HtmlString("<p class=\"govuk-body\">" + HtmlEncoder.Default.Encode(i.Panel.Text) + "</p>") :
                            i.Panel.Html is not null ? new HtmlString(i.Panel.Html) :
                            null
                }))
                    .OrEmpty();

                return generator.GenerateTabs(options.Id, options.IdPrefix, title, attributes, items)
                    .ToHtmlString();
            });
}
