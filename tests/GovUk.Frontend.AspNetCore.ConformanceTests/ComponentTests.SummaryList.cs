using System.Linq;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    [Theory]
    [ComponentFixtureData(
        "summary-list",
        typeof(OptionsJson.SummaryList),
        exclude: new[]
        {
            "with falsy values",
            "as a summary card with actions plus summary list actions"
        })]
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
                        Actions = new SummaryListActions()
                        {
                            Items = (r.Actions?.Items
                                .Select(a => new SummaryListAction()
                                {
                                    Attributes = a.Attributes.ToAttributesDictionary()
                                        .MergeAttribute("class", a.Classes)
                                        .MergeAttribute("href", a.Href),
                                    Content = TextOrHtmlHelper.GetHtmlContent(a.Text, a.Html),
                                    VisuallyHiddenText = a.VisuallyHiddenText is string vht && vht != string.Empty ? vht : null
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

                var summaryList = generator.GenerateSummaryList(rows, attributes);

                if (options.Card != null)
                {
                    return generator
                        .GenerateSummaryCard(
                            options.Card.Title is not null ?
                                new SummaryCardTitle()
                                {
                                    Content = TextOrHtmlHelper.GetHtmlContent(options.Card.Title?.Text, options.Card.Title?.Html),
                                    Attributes = options.Card.Attributes.ToAttributesDictionary(),
                                    HeadingLevel = options.Card.Title.HeadingLevel
                                } :
                                null,
                            new SummaryListActions()
                            {
                                Attributes = new AttributeDictionary().MergeAttribute("class", options.Card.Actions?.Classes),
                                Items = options.Card.Actions?.Items?.Select(a => new SummaryListAction()
                                {
                                    Attributes = a.Attributes.ToAttributesDictionary()
                                        .MergeAttribute("class", a.Classes)
                                        .MergeAttribute("href", a.Href),
                                    Content = TextOrHtmlHelper.GetHtmlContent(a.Text, a.Html),
                                    VisuallyHiddenText = a.VisuallyHiddenText is string vht && vht != string.Empty ? vht : null
                                }).OrEmpty().ToList()
                            },
                            summaryList,
                            options.Card.Attributes.ToAttributesDictionary().MergeAttribute("class", options.Card.Classes))
                        .ToHtmlString();
                }
                else
                {
                    return summaryList.ToHtmlString();
                }
            });
}
