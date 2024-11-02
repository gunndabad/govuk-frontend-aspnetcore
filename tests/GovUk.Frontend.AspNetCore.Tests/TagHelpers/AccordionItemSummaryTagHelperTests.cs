using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class AccordionItemSummaryTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsSummaryToContext()
    {
        // Arrange
        var accordionContext = new AccordionContext();
        var itemContext = new AccordionItemContext();

        var context = new TagHelperContext(
            tagName: "govuk-accordion-item-summary",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(AccordionContext), accordionContext },
                { typeof(AccordionItemContext), itemContext },
            },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-accordion-item-summary",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Summary content");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new AccordionItemSummaryTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(itemContext.Summary);
        Assert.Equal("Summary content", itemContext.Summary?.Content.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_ItemAlreadyHasSummary_ThrowsInvalidOperationException()
    {
        // Arrange
        var accordionContext = new AccordionContext();
        var itemContext = new AccordionItemContext();
        itemContext.SetSummary(new AttributeDictionary(), new HtmlString("Existing summary"));

        var context = new TagHelperContext(
            tagName: "govuk-accordion-item-summary",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(AccordionContext), accordionContext },
                { typeof(AccordionItemContext), itemContext },
            },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-accordion-item-summary",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Summary content");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new AccordionItemSummaryTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal(
            "Only one <govuk-accordion-item-summary> is permitted for each <govuk-accordion-item>.",
            ex.Message
        );
    }
}
