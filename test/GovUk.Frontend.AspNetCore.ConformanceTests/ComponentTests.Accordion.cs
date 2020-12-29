using System.Linq;
using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        [Theory]
        [ComponentFixtureData(
            "accordion",
            typeof(OptionsJson.Accordion),
            exclude: "with falsey values")]
        public void Accordion(ComponentTestCaseData<OptionsJson.Accordion> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var id = options.Id ?? "GFA_test";
                    var headingLevel = options.HeadingLevel.GetValueOrDefault(ComponentGenerator.AccordionDefaultHeadingLevel);

                    var attributes = options.Attributes.ToAttributesDictionary()
                        .MergeAttribute("class", options.Classes);

                    var items = options.Items
                        .Select(i => new AccordionItem()
                        {
                            Content = TextOrHtmlHelper.GetHtmlContent(i.Content.Text, i.Content.Html),
                            Expanded = i.Expanded ?? ComponentGenerator.AccordionItemDefaultExpanded,
                            HeadingContent = i.Heading != null ? TextOrHtmlHelper.GetHtmlContent(i.Heading.Text, i.Heading.Html) : null,
                            SummaryContent = i.Summary != null ? TextOrHtmlHelper.GetHtmlContent(i.Summary.Text, i.Summary.Html) : null
                        })
                        .OrEmpty();

                    return generator.GenerateAccordion(id, headingLevel, attributes, items)
                        .RenderToString();
                },
                excludeDiff: diff => diff is AttrDiff attrDiff &&
                    ((attrDiff.Test.Attribute.Name == "id" && attrDiff.Test.Attribute.Value.StartsWith("GFA_test")) ||
                    (attrDiff.Test.Attribute.Name == "aria-labelledby" && attrDiff.Test.Attribute.Value.StartsWith("GFA_test"))));
    }
}
