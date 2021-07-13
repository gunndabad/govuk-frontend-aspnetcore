using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class AccordionItemHeadingTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsHeadingToContext()
        {
            // Arrange
            var accordionContext = new AccordionContext();
            var itemContext = new AccordionItemContext();

            var context = new TagHelperContext(
                tagName: "govuk-accordion-item-heading",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(AccordionContext), accordionContext },
                    { typeof(AccordionItemContext), itemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion-item-heading",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Summary content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new AccordionItemHeadingTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.NotNull(itemContext.Heading);
            Assert.Equal("Summary content", itemContext.Heading.Value.content.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_ItemAlreadyHasHeader_ThrowsInvalidOperationException()
        {
            // Arrange
            var accordionContext = new AccordionContext();
            var itemContext = new AccordionItemContext();
            itemContext.SetHeading(attributes: null, content: new HtmlString("Existing heading"));

            var context = new TagHelperContext(
                tagName: "govuk-accordion-item-heading",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(AccordionContext), accordionContext },
                    { typeof(AccordionItemContext), itemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion-item-heading",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Summary content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new AccordionItemHeadingTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-accordion-item-heading> is permitted for each <govuk-accordion-item>.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_ItemAlreadyHasSummary_ThrowsInvalidOperationException()
        {
            // Arrange
            var accordionContext = new AccordionContext();
            var itemContext = new AccordionItemContext();
            itemContext.SetSummary(attributes: new Dictionary<string, string>(), content: new HtmlString("Summary"));

            var context = new TagHelperContext(
                tagName: "govuk-accordion-item-heading",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(AccordionContext), accordionContext },
                    { typeof(AccordionItemContext), itemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion-item-heading",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Summary content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new AccordionItemHeadingTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-accordion-item-heading> must be specified before <govuk-accordion-item-summary>.", ex.Message);
        }
    }
}
