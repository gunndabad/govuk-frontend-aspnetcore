using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class SummaryListRowActionsTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsAttributesToContext()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();

            var rowContext = new SummaryListRowContext();

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row-actions",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext },
                    { typeof(SummaryListRowContext), rowContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row-actions",
                attributes: new TagHelperAttributeList()
                {
                    { "class", "additional-class" }
                },
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowActionsTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Collection(
                rowContext.ActionsAttributes,
                kvp =>
                {
                    Assert.Equal("class", kvp.Key);
                    Assert.Equal("additional-class", kvp.Value);
                });
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasActionsAttributes_ThrowsInvalidOperationException()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();

            var rowContext = new SummaryListRowContext();
            rowContext.SetActionsAttributes(new AttributeDictionary());

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row-actions",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext },
                    { typeof(SummaryListRowContext), rowContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row-actions",
                attributes: new TagHelperAttributeList()
                {
                    { "class", "additional-class" }
                },
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowActionsTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-summary-list-row-actions> element is permitted within each <govuk-summary-list-row>.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasAction_ThrowsInvalidOperationException()
        {
            // Arrange
            var summaryListContext = new SummaryListContext();

            var rowContext = new SummaryListRowContext();
            rowContext.AddAction(new HtmlGeneration.SummaryListAction()
            {
                Content = new HtmlString("Action")
            });

            var context = new TagHelperContext(
                tagName: "govuk-summary-list-row-actions",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(SummaryListContext), summaryListContext },
                    { typeof(SummaryListRowContext), rowContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-summary-list-row-actions",
                attributes: new TagHelperAttributeList()
                {
                    { "class", "additional-class" }
                },
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SummaryListRowActionsTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-summary-list-row-actions> must be specified before <govuk-summary-list-row-action>.", ex.Message);
        }
    }
}
