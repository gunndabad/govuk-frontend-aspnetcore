using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class SummaryListRowTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsRowToContext()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var rowContext = (SummaryListRowContext)context.Items[typeof(SummaryListRowContext)];
                    rowContext.SetKey(new HtmlString("Key"), attributes: null);
                    rowContext.SetValue(new HtmlString("Value"), attributes: null);
                    rowContext.AddAction(new SummaryListRowAction()
                    {
                        Attributes = new Dictionary<string, string>()
                        {
                            { "href", "first" }
                        },
                        Content = new HtmlString("First action"),
                        VisuallyHiddenText = "vht1"
                    });
                    rowContext.AddAction(new SummaryListRowAction()
                    {
                        Attributes = new Dictionary<string, string>()
                        {
                            { "href", "second" }
                        },
                        Content = new HtmlString("Second action"),
                        VisuallyHiddenText = "vht2"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(1, summaryListContext.Rows.Count);

            Assert.Collection(
                summaryListContext.Rows,
                row =>
                {
                    Assert.Equal("Key", row.Key.Content.RenderToString());
                    Assert.Equal("Value", row.Value.Content.RenderToString());

                    Assert.Collection(
                        row.Actions.Items,
                        action =>
                        {
                            Assert.Equal("First action", action.Content.RenderToString());
                            Assert.Contains(action.Attributes, kvp => kvp.Key == "href" && kvp.Value == "first");
                            Assert.Equal("vht1", action.VisuallyHiddenText);
                        },
                        action =>
                        {
                            Assert.Equal("Second action", action.Content.RenderToString());
                            Assert.Contains(action.Attributes, kvp => kvp.Key == "href" && kvp.Value == "second");
                            Assert.Equal("vht2", action.VisuallyHiddenText);
                        });
                });
        }

        [Fact]
        public async Task ProcessAsync_RowIsMissingKey_ThrowsInvalidOperationException()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("A <govuk-summary-list-row-key> element must be provided.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_RowIsMissingValue_ThrowsInvalidOperationException()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var rowContext = (SummaryListRowContext)context.Items[typeof(SummaryListRowContext)];
                    rowContext.SetKey(new HtmlString("Key"), attributes: null);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("A <govuk-summary-list-row-value> element must be provided.", ex.Message);
        }
    }
}
