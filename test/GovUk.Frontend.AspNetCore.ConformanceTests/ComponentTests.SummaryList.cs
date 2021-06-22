using System.Collections.Generic;
using System.Linq;
using GovUk.Frontend.AspNetCore.TestCommon;
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
                            Actions = (r.Actions?.Items
                                .Select(a => new SummaryListRowAction()
                                {
                                    Attributes = a.Attributes.ToAttributesDictionary().MergeAttribute("class", a.Classes),
                                    Content = TextOrHtmlHelper.GetHtmlContent(a.Text, a.Html),
                                    Href = a.Href,
                                    VisuallyHiddenText = a.VisuallyHiddenText
                                })).OrEmpty(),
                            Attributes = new Dictionary<string, string>().MergeAttribute("class", r.Classes),
                            Key = TextOrHtmlHelper.GetHtmlContent(r.Key.Text, r.Key.Html),
                            Value = TextOrHtmlHelper.GetHtmlContent(r.Value.Text, r.Value.Html)
                        })
                        .OrEmpty();

                    return generator.GenerateSummaryList(attributes, rows)
                        .RenderToString();
                });
    }
}
