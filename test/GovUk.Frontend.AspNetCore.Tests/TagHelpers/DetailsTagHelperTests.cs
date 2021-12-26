using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class DetailsTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-details",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var detailsContext = (DetailsContext)context.Items[typeof(DetailsContext)];

                    var summary = new HtmlString("The summary");
                    detailsContext.SetSummary(new AttributeDictionary(), summary);

                    var text = new HtmlString("The text");
                    detailsContext.SetText(new AttributeDictionary(), text);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsTagHelper(new ComponentGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<details class=""govuk-details"" data-module=""govuk-details"">
    <summary class=""govuk-details__summary"">
        <span class=""govuk-details__summary-text"">The summary</span>
    </summary>
    <div class=""govuk-details__text"">The text</div>
</details>";

            AssertEx.HtmlEqual(expectedHtml, output.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_WithOpenSpecified_AddsOpenAttributeToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-details",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var detailsContext = (DetailsContext)context.Items[typeof(DetailsContext)];

                    var summary = new HtmlString("The summary");
                    detailsContext.SetSummary(new AttributeDictionary(), summary);

                    var text = new HtmlString("The text");
                    detailsContext.SetText(new AttributeDictionary(), text);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsTagHelper(new ComponentGenerator())
            {
                Open = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var element = output.RenderToElement();

            Assert.Equal("", element.Attributes["open"].Value);
        }

        [Fact]
        public async Task ProcessAsync_MissingSummary_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-details",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var detailsContext = (DetailsContext)context.Items[typeof(DetailsContext)];

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsTagHelper(new ComponentGenerator())
            {
                Open = true
            };

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("A <govuk-details-summary> element must be provided.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_MissingText_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-details",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var detailsContext = (DetailsContext)context.Items[typeof(DetailsContext)];

                    var summary = new HtmlString("The summary");
                    detailsContext.SetSummary(new AttributeDictionary(), summary);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsTagHelper(new ComponentGenerator())
            {
                Open = true
            };

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("A <govuk-details-text> element must be provided.", ex.Message);
        }
    }
}
