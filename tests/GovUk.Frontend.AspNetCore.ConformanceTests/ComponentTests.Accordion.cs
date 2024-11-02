using System.Linq;
using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData("accordion", typeof(OptionsJson.Accordion), exclude: "with falsey values")]
    public void Accordion(ComponentTestCaseData<OptionsJson.Accordion> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) =>
            {
                var headingLevel = options.HeadingLevel.GetValueOrDefault(
                    ComponentGenerator.AccordionDefaultHeadingLevel
                );

                var attributes = options.Attributes.ToAttributesDictionary().MergeAttribute("class", options.Classes);

                var items = options
                    .Items.Select(i => new AccordionItem()
                    {
                        Content =
                            i.Content.Text != null
                                ? new HtmlString(
                                    "<p class=\"govuk-body\">" + HtmlEncoder.Default.Encode(i.Content.Text) + "</p>"
                                )
                            : i.Content.Html != null ? new HtmlString(i.Content.Html)
                            : null,
                        Expanded = i.Expanded ?? ComponentGenerator.AccordionItemDefaultExpanded,
                        HeadingContent =
                            i.Heading != null ? TextOrHtmlHelper.GetHtmlContent(i.Heading.Text, i.Heading.Html) : null,
                        SummaryContent =
                            i.Summary != null ? TextOrHtmlHelper.GetHtmlContent(i.Summary.Text, i.Summary.Html) : null,
                    })
                    .OrEmpty();

                return generator
                    .GenerateAccordion(
                        options.Id,
                        headingLevel,
                        attributes,
                        options.RememberExpanded ?? ComponentGenerator.AccordionDefaultRememberExpanded,
                        options.HideAllSectionsText,
                        options.HideSectionText,
                        options.HideSectionAriaLabelText,
                        options.ShowAllSectionsText,
                        options.ShowSectionText,
                        options.ShowSectionAriaLabelText,
                        items
                    )
                    .ToHtmlString();
            }
        );
}
