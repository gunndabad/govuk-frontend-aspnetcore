﻿using System;
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
    public class AccordionTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-accordion",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var accordionContext = (AccordionContext)context.Items[AccordionContext.ContextName];

                    accordionContext.AddItem(new AccordionItem()
                    {
                        Content = new HtmlString("First content"),
                        Expanded = false,
                        HeadingContent = new HtmlString("First heading"),
                        HeadingLevel = 1,
                        Summary = new HtmlString("First summary")
                    });

                    accordionContext.AddItem(new AccordionItem()
                    {
                        Content = new HtmlString("First content"),
                        Expanded = true,
                        HeadingContent = new HtmlString("Second heading"),
                        HeadingLevel = 2
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new AccordionTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>()))
            {
                Id = "testaccordion"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(
                "<div class=\"govuk-accordion\" data-module=\"govuk-accordion\" id=\"testaccordion\">" +
                "<div class=\"govuk-accordion__section\">" +
                "<div class=\"govuk-accordion__section-header\">" +
                "<h1 class=\"govuk-accordion__section-heading\">" +
                "<span class=\"govuk-accordion__section-button\" id=\"testaccordion-heading-0\">First heading</span>" +
                "</h1>" +
                "<div class=\"govuk-body govuk-accordion__section-summary\" id=\"testaccordion-summary-0\">" +
                "</div>" +
                "</div>" +
                "<div aria-labelledby=\"testaccordion-heading-0\" class=\"govuk-accordion__section-content\" id=\"testaccordion-content-0\">" +
                "First content" +
                "</div>" +
                "</div>" +
                "<div class=\"govuk-accordion__section--expanded govuk-accordion__section\">" +
                "<div class=\"govuk-accordion__section-header\"><h2 class=\"govuk-accordion__section-heading\">" +
                "<span class=\"govuk-accordion__section-button\" id=\"testaccordion-heading-1\">Second heading</span>" +
                "</h2>" +
                "</div>" +
                "<div aria-labelledby=\"testaccordion-heading-1\" class=\"govuk-accordion__section-content\" id=\"testaccordion-content-1\">" +
                "First content" +
                "</div>" +
                "</div>" +
                "</div>",
                output.AsString());
        }

        [Fact]
        public async Task ProcessAsync_NoIdThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-accordion",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var accordionContext = (AccordionContext)context.Items[AccordionContext.ContextName];

                    accordionContext.AddItem(new AccordionItem()
                    {
                        Content = new HtmlString("First content"),
                        Expanded = false,
                        HeadingContent = new HtmlString("First heading"),
                        HeadingLevel = 1,
                        Summary = new HtmlString("First summary")
                    });

                    accordionContext.AddItem(new AccordionItem()
                    {
                        Content = new HtmlString("First content"),
                        Expanded = true,
                        HeadingContent = new HtmlString("Second heading"),
                        HeadingLevel = 2
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new AccordionTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>()));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("The 'id' attribute must be specified.", ex.Message);
        }
    }

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
                    { AccordionContext.ContextName, accordionContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var itemContext = (AccordionItemContext)context.Items[AccordionItemContext.ContextName];
                    itemContext.TrySetHeading(1, new HtmlString("Heading"));
                    itemContext.TrySetSummary(new HtmlString("Summary"));

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
            Assert.Equal(1, firstItem.HeadingLevel.Value);
            Assert.Equal("Heading", firstItem.HeadingContent.AsString());
            Assert.Equal("Summary", firstItem.Summary.AsString());
            Assert.Equal("Content", firstItem.Content.AsString());
        }

        [Fact]
        public async Task ProcessAsync_NoHeadingThrowsInvalidOperationException()
        {
            // Arrange
            var accordionContext = new AccordionContext();

            var context = new TagHelperContext(
                tagName: "govuk-accordion-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { AccordionContext.ContextName, accordionContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var itemContext = (AccordionItemContext)context.Items[AccordionItemContext.ContextName];
                    itemContext.TrySetSummary(new HtmlString("Summary"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new AccordionItemTagHelper();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Missing <govuk-accordion-item-heading> element.", ex.Message);
        }
    }

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
                    { AccordionContext.ContextName, accordionContext },
                    { AccordionItemContext.ContextName, itemContext }
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

            var tagHelper = new AccordionItemHeadingTagHelper()
            {
                Level = 3
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.NotNull(itemContext.Heading);
            Assert.Equal(3, itemContext.Heading.Value.level);
            Assert.Equal("Summary content", itemContext.Heading.Value.content.AsString());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(7)]
        public async Task ProcessAsync_InvalidLevelThrowsInvalidOperationException(int level)
        {
            // Arrange
            var accordionContext = new AccordionContext();
            var itemContext = new AccordionItemContext();

            var context = new TagHelperContext(
                tagName: "govuk-accordion-item-heading",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { AccordionContext.ContextName, accordionContext },
                    { AccordionItemContext.ContextName, itemContext }
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

            var tagHelper = new AccordionItemHeadingTagHelper()
            {
                Level = level
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("The 'level' attribute must be between 1 and 6.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_ItemAlreadyHasHeaderThrowsInvalidOperationException()
        {
            // Arrange
            var accordionContext = new AccordionContext();
            var itemContext = new AccordionItemContext();
            itemContext.TrySetHeading(level: null, content: new HtmlString("Existing heading"));

            var context = new TagHelperContext(
                tagName: "govuk-accordion-item-heading",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { AccordionContext.ContextName, accordionContext },
                    { AccordionItemContext.ContextName, itemContext }
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

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot render <govuk-accordion-item-heading> here.", ex.Message);
        }
    }

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
                    { AccordionContext.ContextName, accordionContext },
                    { AccordionItemContext.ContextName, itemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion-item-summary",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Summary content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new AccordionItemSummaryTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.NotNull(itemContext.Summary);
            Assert.Equal("Summary content", itemContext.Summary.AsString());
        }

        [Fact]
        public async Task ProcessAsync_ItemAlreadyHasSummaryThrowsInvalidOperationException()
        {
            // Arrange
            var accordionContext = new AccordionContext();
            var itemContext = new AccordionItemContext();
            itemContext.TrySetSummary(new HtmlString("Existing summary"));

            var context = new TagHelperContext(
                tagName: "govuk-accordion-item-summary",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { AccordionContext.ContextName, accordionContext },
                    { AccordionItemContext.ContextName, itemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-accordion-item-summary",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Summary content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new AccordionItemSummaryTagHelper();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot render <govuk-accordion-item-summary> here.", ex.Message);
        }
    }
}
