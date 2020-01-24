using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class BreadcrumbsTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-breadcrumbs",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-breadcrumbs",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var bcContext = (BreadcrumbsContext)context.Items["BreadcrumbsContext"];

                    var item1 = new HtmlString("<a href=\"first\" class=\"govuk-breadcrumbs__link\">First</a>");
                    bcContext.AddItem(item1);

                    var item2 = new HtmlString("<a href=\"second\" class=\"govuk-breadcrumbs__link\">Second</a>");
                    bcContext.AddItem(item2);

                    var item3 = new HtmlString("Last");
                    bcContext.AddCurrentPageItem(item3);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new BreadcrumbsTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>()));

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-breadcrumbs\">" +
                "<ol class=\"govuk-breadcrumbs__list\">" +
                "<li class=\"govuk-breadcrumbs__list-item\"><a href=\"first\" class=\"govuk-breadcrumbs__link\">First</a></li>" +
                "<li class=\"govuk-breadcrumbs__list-item\"><a href=\"second\" class=\"govuk-breadcrumbs__link\">Second</a></li>" +
                "<li aria-current=\"page\" class=\"govuk-breadcrumbs__list-item\">Last</li>" +
                "</ol>" +
                "</div>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_WithoutCurrentPageItemGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-breadcrumbs",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-breadcrumbs",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var bcContext = (BreadcrumbsContext)context.Items["BreadcrumbsContext"];

                    var item1 = new HtmlString("<a href=\"first\" class=\"govuk-breadcrumbs__link\">First</a>");
                    bcContext.AddItem(item1);

                    var item2 = new HtmlString("<a href=\"second\" class=\"govuk-breadcrumbs__link\">Second</a>");
                    bcContext.AddItem(item2);

                    var item3 = new HtmlString("Last");
                    bcContext.AddItem(item3);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new BreadcrumbsTagHelper(new DefaultGovUkHtmlGenerator(Mock.Of<IUrlHelperFactory>()));

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-breadcrumbs\">" +
                "<ol class=\"govuk-breadcrumbs__list\">" +
                "<li class=\"govuk-breadcrumbs__list-item\"><a href=\"first\" class=\"govuk-breadcrumbs__link\">First</a></li>" +
                "<li class=\"govuk-breadcrumbs__list-item\"><a href=\"second\" class=\"govuk-breadcrumbs__link\">Second</a></li>" +
                "<li class=\"govuk-breadcrumbs__list-item\">Last</li>" +
                "</ol>" +
                "</div>",
                html);
        }
    }
}
