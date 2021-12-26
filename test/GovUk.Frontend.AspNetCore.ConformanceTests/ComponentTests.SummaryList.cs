using System.Linq;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData(
            "summary-list",
            typeof(OptionsJson.SummaryList),
            exclude: "with falsey values")]
        public void SummaryList(ComponentTestCaseData<OptionsJson.SummaryList> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var attributes = options.Attributes.ToAttributesDictionary()
                        .MergeAttribute("class", options.Classes);

                    var rows = options.Rows
                        .Select(r => new SummaryListRow()
                        {
                            Actions = new SummaryListRowActions()
                            {
                                Items = (r.Actions?.Items
                                    .Select(a => new SummaryListRowAction()
                                    {
                                        Attributes = a.Attributes.ToAttributesDictionary()
                                            .MergeAttribute("class", a.Classes)
                                            .MergeAttribute("href", a.Href),
                                        Content = TextOrHtmlHelper.GetHtmlContent(a.Text, a.Html),
                                        VisuallyHiddenText = a.VisuallyHiddenText
                                    })).OrEmpty().ToList(),
                                Attributes = new AttributeDictionary().MergeAttribute("class", r.Actions?.Classes)
                            },
                            Attributes = new AttributeDictionary().MergeAttribute("class", r.Classes),
                            Key = new SummaryListRowKey()
                            {
                                Content = TextOrHtmlHelper.GetHtmlContent(r.Key.Text, r.Key.Html),
                                Attributes = new AttributeDictionary().MergeAttribute("class", r.Key.Classes)
                            },
                            Value = new SummaryListRowValue()
                            {
                                Content = TextOrHtmlHelper.GetHtmlContent(r.Value.Text, r.Value.Html),
                                Attributes = new AttributeDictionary().MergeAttribute("class", r.Value.Classes)
                            }
                        })
                        .OrEmpty();

                    return generator.GenerateSummaryList(attributes, rows)
                        .RenderToString();
                });
    }
}
