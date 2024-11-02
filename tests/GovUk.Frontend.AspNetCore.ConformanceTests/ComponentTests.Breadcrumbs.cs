using System.Linq;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("breadcrumbs", typeof(OptionsJson.Breadcrumbs))]
    public void Breadcrumbs(ComponentTestCaseData<OptionsJson.Breadcrumbs> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var collapseOnMobile =
                    options.CollapseOnMobile ?? ComponentGenerator.BreadcrumbsDefaultCollapseOnMobile;

                var attributes = options.Attributes.ToAttributesDictionary().MergeAttribute("class", options.Classes);

                var items = options.Items.Select(i => new BreadcrumbsItem()
                {
                    Content = TextOrHtmlHelper.GetHtmlContent(i.Text, i.Html),
                    Href = i.Href,
                    LinkAttributes = i.Attributes.ToAttributesDictionary(),
                });

                return generator.GenerateBreadcrumbs(collapseOnMobile, attributes, items).ToHtmlString();
            }
        );
}
