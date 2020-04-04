using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
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
                        Key = new HtmlString("Row 1 key"),
                        Value = new HtmlString("Row 1 value"),
                        Actions = new[]
                        {
                            new SummaryListRowAction()
                            {
                                Content = new HtmlString("Row 1 action 1 content"),
                                Href = "row1action1",
                                VisuallyHiddenText = "row1action1vht"
                            },
                            new SummaryListRowAction()
                            {
                                Content = new HtmlString("Row 1 action 2 content"),
                                Href = "row1action2",
                                VisuallyHiddenText = "row1action2vht"
                            }
                        }
                    });

                    summaryListContent.AddRow(new SummaryListRow()
                    {
                        Key = new HtmlString("Row 2 key"),
                        Value = new HtmlString("Row 2 value"),
                        Actions = Enumerable.Empty<SummaryListRowAction>()
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListTagHelper(new DefaultGovUkHtmlGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(
                "<dl class=\"govuk-summary-list\">" +
                "<div class=\"govuk-summary-list__row\">" +
                "<dt class=\"govuk-summary-list__key\">Row 1 key</dt>" +
                "<dt class=\"govuk-summary-list__value\">Row 1 value</dt>" +
                "<dd class=\"govuk-summary-list__actions\">" +
                "<ul class=\"govuk-summary-list__actions-list\">" +
                "<li class=\"govuk-summary-list__actions-list-item\">" +
                "<a class=\"govuk-link\" href=\"row1action1\">Row 1 action 1 content<span class=\"govuk-visually-hidden\">row1action1vht</span></a>" +
                "</li>" +
                "<li class=\"govuk-summary-list__actions-list-item\">" +
                "<a class=\"govuk-link\" href=\"row1action2\">Row 1 action 2 content<span class=\"govuk-visually-hidden\">row1action2vht</span></a>" +
                "</li>" +
                "</ul>" +
                "</dd>" +
                "</div>" +
                "<div class=\"govuk-summary-list__row\">" +
                "<dt class=\"govuk-summary-list__key\">Row 2 key</dt>" +
                "<dt class=\"govuk-summary-list__value\">Row 2 value</dt>" +
                "<span class=\"govuk-summary-list__actions\"></span>" +
                "</div>" +
                "</dl>",
                output.AsString());
        }
    }

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
                    rowContext.TrySetKey(new HtmlString("Key"));
                    rowContext.TrySetValue(new HtmlString("Value"));
                    rowContext.AddAction(new SummaryListRowAction()
                    {
                        Content = new HtmlString("First action"),
                        Href = "first",
                        VisuallyHiddenText = "vht1"
                    });
                    rowContext.AddAction(new SummaryListRowAction()
                    {
                        Content = new HtmlString("Second action"),
                        Href = "second",
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

            var firstRow = summaryListContext.Rows.First();
            Assert.Equal("Key", firstRow.Key.AsString());
            Assert.Equal("Value", firstRow.Value.AsString());
            Assert.Equal(2, firstRow.Actions.Count());
            Assert.Equal("First action", firstRow.Actions.First().Content.AsString());
            Assert.Equal("first", firstRow.Actions.First().Href);
            Assert.Equal("vht1", firstRow.Actions.First().VisuallyHiddenText);
            Assert.Equal("Second action", firstRow.Actions.Skip(1).First().Content.AsString());
            Assert.Equal("second", firstRow.Actions.Skip(1).First().Href);
            Assert.Equal("vht2", firstRow.Actions.Skip(1).First().VisuallyHiddenText);
        }
    }

    public class SummaryListRowKeyTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsValueToContext()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();
            var rowContext = new SummaryListRowContext();

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row-key",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext },
                    { typeof(SummaryListRowContext), rowContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row-key",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Key content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowKeyTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Key content", rowContext.Key.AsString());
        }

        [Fact]
        public async Task ProcessAsync_RowAlreadyHasValueThrowsInvalidOperationException()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();
            var rowContext = new SummaryListRowContext();
            rowContext.TrySetKey(new HtmlString("Existing key"));

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row-key",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext },
                    { typeof(SummaryListRowContext), rowContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row-key",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Key content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowKeyTagHelper();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot render <govuk-summary-list-row-key> here.", ex.Message);
        }
    }

    public class SummaryListRowValueTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsValueToContext()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();
            var rowContext = new SummaryListRowContext();

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row-value",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext },
                    { typeof(SummaryListRowContext), rowContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row-value",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Value content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowValueTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Value content", rowContext.Value.AsString());
        }

        [Fact]
        public async Task ProcessAsync_RowAlreadyHasValueThrowsInvalidOperationException()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();
            var rowContext = new SummaryListRowContext();
            rowContext.TrySetValue(new HtmlString("Existing value"));

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row-value",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext },
                    { typeof(SummaryListRowContext), rowContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row-value",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Value content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowValueTagHelper();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot render <govuk-summary-list-row-value> here.", ex.Message);
        }
    }

    public class SummaryListRowActionTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsActionToContext()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();
            var rowContext = new SummaryListRowContext();

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row-action",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext },
                    { typeof(SummaryListRowContext), rowContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row-action",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Action content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowActionTagHelper(
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
                Href = "href",
                VisuallyHiddenText = "vht"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(1, rowContext.Actions.Count);

            var firstAction = rowContext.Actions.First();
            Assert.Equal("vht", firstAction.VisuallyHiddenText);
            Assert.Equal("href", firstAction.Href);
            Assert.Equal("Action content", firstAction.Content.AsString());
        }
    }
}
