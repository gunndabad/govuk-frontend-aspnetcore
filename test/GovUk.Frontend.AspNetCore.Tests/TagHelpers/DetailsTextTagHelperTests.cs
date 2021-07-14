using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class DetailsTextTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsContentOnContext()
        {
            // Arrange
            var detailsContext = new DetailsContext();
            detailsContext.SetSummary(Attributes.Empty(), new HtmlString("The summary"));

            var context = new TagHelperContext(
                tagName: "govuk-details-text",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(DetailsContext), detailsContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details-text",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsTextTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("The text", detailsContext.Text?.Content.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_ParentAlreadyHasText_ThrowsInvalidOperationException()
        {
            // Arrange
            var detailsContext = new DetailsContext();
            detailsContext.SetSummary(Attributes.Empty(), new HtmlString("The summary"));
            detailsContext.SetText(Attributes.Empty(), new HtmlString("The text"));

            var context = new TagHelperContext(
                tagName: "govuk-details-text",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(DetailsContext), detailsContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-details-text",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DetailsTextTagHelper();

            // Act
            var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-details-text> element is permitted within each <govuk-details>.", ex.Message);
        }
    }
}
