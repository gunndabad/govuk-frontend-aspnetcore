using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

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
                itemContext.SetContent(new AttributeDictionary(), new HtmlString("Content"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new AccordionItemTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var firstItem = Assert.Single(accordionContext.Items);
        Assert.Equal("Heading", firstItem.HeadingContent?.ToHtmlString());
        Assert.Equal("Summary", firstItem.SummaryContent?.ToHtmlString());
        Assert.Equal("Content", firstItem.Content?.ToHtmlString());
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
                itemContext.SetContent(new AttributeDictionary(), new HtmlString("Content"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new AccordionItemTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("A <govuk-accordion-item-heading> element must be provided.", ex.Message);
    }

    [Fact]
    public async Task ProcessAsync_NoContent_ThrowsInvalidOperationException()
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

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new AccordionItemTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("A <govuk-accordion-item-content> element must be provided.", ex.Message);
    }
}
