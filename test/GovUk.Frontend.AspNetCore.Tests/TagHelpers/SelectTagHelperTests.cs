using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class SelectTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-select",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-select",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label"));

                    var selectContext = (SelectContext)context.Items[SelectContext.ContextName];

                    selectContext.AddItem(new SelectListItem()
                    {
                        Content = new HtmlString("First")
                    });

                    selectContext.AddItem(new SelectListItem()
                    {
                        Content = new HtmlString("Second"),
                        Value = "second"
                    });

                    selectContext.AddItem(new SelectListItem()
                    {
                        Content = new HtmlString("Third"),
                        Disabled = true,
                        Value = "third"
                    });

                    selectContext.AddItem(new SelectListItem()
                    {
                        Content = new HtmlString("Fourth"),
                        Selected = true,
                        Value = "fourth"
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SelectTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            var input = node.ChildNodes.FindFirst("select");
            Assert.Equal(
                "<select aria-describedby=\"describedby\" class=\"govuk-select\" id=\"my-id\" name=\"my-id\">" +
                "<option>First</option>" +
                "<option value=\"second\">Second</option>" +
                "<option disabled=\"disabled\" value=\"third\">Third</option>" +
                "<option selected=\"selected\" value=\"fourth\">Fourth</option>" +
                "</select>",
                input.OuterHtml);
        }

        [Fact]
        public async Task ProcessAsync_HasErrorClassWhenErrorSpecified()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-select",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-select",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        content: new HtmlString("The label")); formGroupContext.TrySetErrorMessage(
                         visuallyHiddenText: null,
                         attributes: null,
                         content: new HtmlString("Error"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new SelectTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "my-id",
                DescribedBy = "describedby",
                Name = "my-id"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            Assert.Contains("govuk-select--error", node.ChildNodes.FindFirst("select").GetCssClasses());
        }
    }
}
