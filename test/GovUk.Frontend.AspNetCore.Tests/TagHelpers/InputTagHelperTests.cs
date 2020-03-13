using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class InputTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(isPageHeading: false, content: new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id",
                Autocomplete = "none",
                InputMode = "numeric",
                Pattern = "[0-9]*",
                Type = "number",
                Value = "42"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            var input = node.ChildNodes.FindFirst("input");
            Assert.Equal(
                "<input aria-describedby=\"describedby\" autocomplete=\"none\" class=\"govuk-input\" id=\"my-id\" inputmode=\"numeric\" name=\"my-id\" pattern=\"[0-9]*\" type=\"number\" value=\"42\">",
                input.OuterHtml);
        }

        [Fact]
        public async Task ProcessAsync_HasErrorClassWhenErrorSpecified()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(isPageHeading: false, content: new HtmlString("The label"));
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        content: new HtmlString("Error"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id",
                Autocomplete = "none",
                InputMode = "numeric",
                Pattern = "[0-9]*",
                Type = "number",
                Value = "42"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            Assert.Contains("govuk-input--error", node.ChildNodes.FindFirst("input").GetCssClasses());
        }

        [Fact]
        public async Task ProcessAsync_NoTypeSpecifiedUsesDefaultType()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-input",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(isPageHeading: false, content: new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id",
                Autocomplete = "none",
                InputMode = "numeric",
                Pattern = "[0-9]*",
                Value = "42"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            var input = node.ChildNodes.FindFirst("input");
            Assert.Equal("text", input.Attributes["type"].Value);
        }
    }
}
