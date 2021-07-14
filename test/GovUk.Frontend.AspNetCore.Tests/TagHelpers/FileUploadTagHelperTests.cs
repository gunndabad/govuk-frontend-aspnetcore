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
    public class FileUploadTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-file-upload",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-file-upload",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new FileUploadTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            var input = node.ChildNodes.FindFirst("input");
            Assert.Equal(
                "<input aria-describedby=\"describedby\" class=\"govuk-file-upload\" id=\"my-id\" name=\"my-id\" type=\"file\">",
                input.OuterHtml);
        }

        [Fact]
        public async Task ProcessAsync_HasErrorClassWhenErrorSpecified()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-file-upload",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-file-upload",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[typeof(FormGroupBuilder)];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        content: new HtmlString("Error"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new FileUploadTagHelper(new ComponentGenerator(), new DefaultModelHelper())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.RenderToString();
            var node = HtmlNode.CreateNode(html);
            Assert.Contains("govuk-file-upload--error", node.ChildNodes.FindFirst("input").GetCssClasses());
        }
    }
}
