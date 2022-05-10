using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class ErrorSummaryTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-summary",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

                    errorSummaryContext.SetTitle(attributes: null, new HtmlString("Title"));
                    errorSummaryContext.SetDescription(attributes: null, new HtmlString("Description"));

                    errorSummaryContext.AddItem(new ErrorSummaryItem()
                    {
                        Content = new HtmlString("First message"),
                        Href = "#Field1"
                    });

                    errorSummaryContext.AddItem(new ErrorSummaryItem()
                    {
                        Content = new HtmlString("Second message"),
                        Href = "#Field2"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div aria-labelledby=""error-summary-title"" class=""govuk-error-summary"" data-module=""govuk-error-summary"" role=""alert"">
    <h2 class=""govuk-error-summary__title"" id=""error-summary-title"">Title</h2>
    <div class=""govuk-error-summary__body"">
        <p>Description</p>
        <ul class=""govuk-error-summary__list govuk-list"">
            <li><a href=""#Field1"">First message</a></li>
            <li><a href=""#Field2"">Second message</a></li>
        </ul>
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_WithDisableAutoFocus_RendersDataAttribute()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-summary",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

                    errorSummaryContext.SetTitle(attributes: null, new HtmlString("Title"));
                    errorSummaryContext.SetDescription(attributes: null, new HtmlString("Description"));

                    errorSummaryContext.AddItem(new ErrorSummaryItem()
                    {
                        Content = new HtmlString("First message"),
                        Href = "#Field1"
                    });

                    errorSummaryContext.AddItem(new ErrorSummaryItem()
                    {
                        Content = new HtmlString("Second message"),
                        Href = "#Field2"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryTagHelper()
            {
                DisableAutoFocus = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var element = output.RenderToElement();
            Assert.NotNull(element.GetAttribute("data-disable-auto-focus"));
        }

        [Fact]
        public async Task ProcessAsync_ItemWithEmptyLink_DoesNotRenderAnchorTag()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-summary",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

                    errorSummaryContext.SetTitle(attributes: null, new HtmlString("Title"));
                    errorSummaryContext.SetDescription(attributes: null, new HtmlString("Description"));

                    errorSummaryContext.AddItem(new ErrorSummaryItem()
                    {
                        Content = new HtmlString("Message"),
                        Href = null
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var element = output.RenderToElement();
            var itemElement = element.QuerySelector("li");
            Assert.NotEqual("a", itemElement.FirstChild.NodeName, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ProcessAsync_NoTitleSpecified_UsesDefaultTitle()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-summary",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

                    errorSummaryContext.AddItem(new ErrorSummaryItem()
                    {
                        Content = new HtmlString("First message")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryTagHelper(new ComponentGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            var h2 = node.ChildNodes.FindFirst("h2");
            Assert.Equal("There is a problem", h2.InnerHtml);
        }

        [Fact]
        public async Task ProcessAsync_NoTitleDescriptionOrItems_RendersNothing()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-summary",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryTagHelper(new ComponentGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            Assert.Empty(html);
        }
    }
}
