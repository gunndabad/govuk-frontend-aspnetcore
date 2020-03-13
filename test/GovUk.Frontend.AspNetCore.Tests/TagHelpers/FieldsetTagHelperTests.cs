using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class FieldsetTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-fieldset",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-fieldset",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var fieldsetContext = (FieldsetContext)context.Items[FieldsetContext.ContextName];
                    fieldsetContext.TrySetLegend(
                        attributes: null,
                        content: new HtmlString("Legend text"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Main content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new FieldsetTagHelper(new DefaultGovUkHtmlGenerator())
            {
                DescribedBy = "describedby",
                Role = "therole"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<fieldset aria-describedby=\"describedby\" class=\"govuk-fieldset\" role=\"therole\">" +
                "<legend class=\"govuk-fieldset__legend\">" +
                "Legend text" +
                "</legend>" +
                "Main content" +
                "</fieldset>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_IsPageHeadingGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-fieldset",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-fieldset",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var fieldsetContext = (FieldsetContext)context.Items[FieldsetContext.ContextName];
                    fieldsetContext.TrySetLegend(
                        attributes: null,
                        content: new HtmlString("Legend text"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Main text");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new FieldsetTagHelper(new DefaultGovUkHtmlGenerator())
            {
                DescribedBy = "describedby",
                IsPageHeading = true,
                Role = "therole"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<fieldset aria-describedby=\"describedby\" class=\"govuk-fieldset\" role=\"therole\">" +
                "<legend class=\"govuk-fieldset__legend\">" +
                "<h1 class=\"govuk-fieldset__heading\">Legend text</h1>" +
                "</legend>" +
                "Main text" +
                "</fieldset>",
                html);
        }
    }
}
