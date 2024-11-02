using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class AccordionItemContentTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsContentToContext()
    {
        // Arrange
        var accordionContext = new AccordionContext();
        var itemContext = new AccordionItemContext();

        var context = new TagHelperContext(
            tagName: "govuk-accordion-item-content",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(AccordionContext), accordionContext },
                { typeof(AccordionItemContext), itemContext },
            },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-accordion-item-content",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Content");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new AccordionItemContentTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(itemContext.Content);
        Assert.Equal("Content", itemContext.Content?.Content?.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_ItemAlreadyHasContent_ThrowsInvalidOperationException()
    {
        // Arrange
        var accordionContext = new AccordionContext();
        var itemContext = new AccordionItemContext();
        itemContext.SetContent(new AttributeDictionary(), content: new HtmlString("Existing content"));

        var context = new TagHelperContext(
            tagName: "govuk-accordion-item-content",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(AccordionContext), accordionContext },
                { typeof(AccordionItemContext), itemContext },
            },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-accordion-item-content",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Content");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new AccordionItemContentTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal(
            "Only one <govuk-accordion-item-content> is permitted for each <govuk-accordion-item>.",
            ex.Message
        );
    }
}
