using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class AccordionItemTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsItemToContext()
        {
            // Arrange
            var accordionContext = new AccordionContext();

            var context = new TagHelperContext(
                tagName: "govuk-accordion-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(AccordionContext), accordionContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var itemContext = context.GetContextItem<AccordionItemContext>();
                    itemContext.SetHeading(new AttributeDictionary(), new HtmlString("Heading"));
                    itemContext.SetSummary(new AttributeDictionary(), new HtmlString("Summary"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new AccordionItemTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(1, accordionContext.Items.Count);

            var firstItem = accordionContext.Items.First();
            Assert.Equal("Heading", firstItem.HeadingContent.RenderToString());
            Assert.Equal("Summary", firstItem.SummaryContent.RenderToString());
            Assert.Equal("Content", firstItem.Content.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_NoHeading_ThrowsInvalidOperationException()
        {
            // Arrange
            var accordionContext = new AccordionContext();

            var context = new TagHelperContext(
                tagName: "govuk-accordion-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(AccordionContext), accordionContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var itemContext = context.GetContextItem<AccordionItemContext>();
                    itemContext.SetSummary(new AttributeDictionary(), new HtmlString("Summary"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new AccordionItemTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("A <govuk-accordion-item-heading> element must be provided.", ex.Message);
        }
    }
}
