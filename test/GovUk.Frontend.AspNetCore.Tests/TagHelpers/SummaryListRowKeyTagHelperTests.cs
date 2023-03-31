using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
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
            Assert.Equal("Key content", rowContext.Key?.Content?.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasKey_ThrowsInvalidOperationException()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();

            var rowContext = new SummaryListRowContext();
            rowContext.SetKey(new AttributeDictionary(), new HtmlString("Key"));

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
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-summary-list-row-key> element is permitted within each <govuk-summary-list-row>.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasValue_ThrowsInvalidOperationException()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();

            var rowContext = new SummaryListRowContext();
            rowContext.SetValue(new AttributeDictionary(), new HtmlString("Value"));

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
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-summary-list-row-key> must be specified before <govuk-summary-list-row-value>.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasActionsAttributes_ThrowsInvalidOperationException()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();

            var rowContext = new SummaryListRowContext();
            rowContext.SetActionsAttributes(new AttributeDictionary());

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
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-summary-list-row-key> must be specified before <govuk-summary-list-row-actions>.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasAction_ThrowsInvalidOperationException()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();

            var rowContext = new SummaryListRowContext();
            rowContext.AddAction(new HtmlGeneration.SummaryListRowAction()
            {
                Content = new HtmlString("Action")
            });

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
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-summary-list-row-key> must be specified before <govuk-summary-list-row-action>.", ex.Message);
        }
    }
}
