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
    public class TextAreaTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-textarea",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-textarea",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (TextAreaBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));
                    formGroupContext.TrySetElementContent(new HtmlString("The content"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TextAreaTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id",
                Rows = 6,
                Autocomplete = "username"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            var textarea = node.ChildNodes.FindFirst("textarea");
            Assert.Equal(
                "<textarea aria-describedby=\"describedby\" autocomplete=\"username\" class=\"govuk-textarea\" id=\"my-id\" name=\"my-id\" rows=\"6\">" +
                "The content" +
                "</textarea>",
                textarea.OuterHtml);
        }

        [Fact]
        public async Task ProcessAsync_NoContentOrAspForThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-textarea",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-textarea",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (TextAreaBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TextAreaTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id",
                Rows = 6,
                Autocomplete = "username"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Content must be specified when the 'asp-for' attribute is not specified.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_HasErrorClassWhenErrorSpecified()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-textarea",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-textarea",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (TextAreaBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        content: new HtmlString("Error"));
                    formGroupContext.TrySetElementContent(new HtmlString("The content"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TextAreaTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id",
                Rows = 6,
                Autocomplete = "username"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            Assert.Contains("govuk-textarea--error", node.ChildNodes.FindFirst("textarea").GetCssClasses());
        }

        [Fact]
        public async Task ProcessAsync_NoRowsSpecifiedUsesDefaultRows()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-textarea",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-textarea",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (TextAreaBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));
                    formGroupContext.TrySetElementContent(new HtmlString("The content"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TextAreaTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id",
                Autocomplete = "username"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            var textarea = node.ChildNodes.FindFirst("textarea");
            Assert.Equal("5", textarea.Attributes["rows"].Value);
        }

        public class Model
        {
            public string Foo { get; set; }
        }
    }
}
