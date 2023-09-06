using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class SummaryListTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-summary-list",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var summaryListContent = (SummaryListContext)context.Items[typeof(SummaryListContext)];

                    summaryListContent.AddRow(new SummaryListRow()
                    {
                        Key = new SummaryListRowKey()
                        {
                            Content = new HtmlString("Row 1 key")
                        },
                        Value = new SummaryListRowValue()
                        {
                            Content = new HtmlString("Row 1 value")
                        },
                        Actions = new SummaryListActions()
                        {
                            Items = new[]
                            {
                                new SummaryListAction()
                                {
                                    Attributes = new AttributeDictionary()
                                    {
                                        { "href", "row1action1" }
                                    },
                                    Content = new HtmlString("Row 1 action 1 content"),
                                    VisuallyHiddenText = "row1action1vht"
                                },
                                new SummaryListAction()
                                {
                                    Attributes = new AttributeDictionary()
                                    {
                                        { "href", "row1action2" }
                                    },
                                    Content = new HtmlString("Row 1 action 2 content"),
                                    VisuallyHiddenText = "row1action2vht"
                                }
                            }
                        }
                    });

                    summaryListContent.AddRow(new SummaryListRow()
                    {
                        Key = new SummaryListRowKey()
                        {
                            Content = new HtmlString("Row 2 key")
                        },
                        Value = new SummaryListRowValue()
                        {
                            Content = new HtmlString("Row 2 value")
                        }
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListTagHelper(new ComponentGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<dl class=""govuk-summary-list"">
    <div class=""govuk-summary-list__row"">
        <dt class=""govuk-summary-list__key"">Row 1 key</dt>
        <dd class=""govuk-summary-list__value"">Row 1 value</dd>
        <dd class=""govuk-summary-list__actions"">
            <ul class=""govuk-summary-list__actions-list"">
                <li class=""govuk-summary-list__actions-list-item"">
                    <a class=""govuk-link"" href=""row1action1"">Row 1 action 1 content<span class=""govuk-visually-hidden"">row1action1vht</span></a>
                </li>
                <li class=""govuk-summary-list__actions-list-item"">
                    <a class=""govuk-link"" href=""row1action2"">Row 1 action 2 content<span class=""govuk-visually-hidden"">row1action2vht</span></a>
                </li>
            </ul>
        </dd>
    </div>
    <div class=""govuk-summary-list__row govuk-summary-list__row--no-actions"">
        <dt class=""govuk-summary-list__key"">Row 2 key</dt>
        <dd class=""govuk-summary-list__value"">Row 2 value</dd>
    </div>
</dl>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_NoRowHasActions_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-summary-list",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var summaryListContent = (SummaryListContext)context.Items[typeof(SummaryListContext)];

                    summaryListContent.AddRow(new SummaryListRow()
                    {
                        Key = new SummaryListRowKey()
                        {
                            Content = new HtmlString("Row 1 key")
                        },
                        Value = new SummaryListRowValue()
                        {
                            Content = new HtmlString("Row 1 value")
                        }
                    });

                    summaryListContent.AddRow(new SummaryListRow()
                    {
                        Key = new SummaryListRowKey()
                        {
                            Content = new HtmlString("Row 2 key")
                        },
                        Value = new SummaryListRowValue()
                        {
                            Content = new HtmlString("Row 2 value")
                        }
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListTagHelper(new ComponentGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<dl class=""govuk-summary-list"">
    <div class=""govuk-summary-list__row"">
        <dt class=""govuk-summary-list__key"">Row 1 key</dt>
        <dd class=""govuk-summary-list__value"">Row 1 value</dd>
    </div>
    <div class=""govuk-summary-list__row"">
        <dt class=""govuk-summary-list__key"">Row 2 key</dt>
        <dd class=""govuk-summary-list__value"">Row 2 value</dd>
    </div>
</dl>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }
    }
}
