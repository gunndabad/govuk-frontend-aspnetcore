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
                    var bcContext = (BreadcrumbsContext)context.Items[typeof(BreadcrumbsContext)];

                    bcContext.AddItem(new BreadcrumbsItem()
                    {
                        Href = "first",
                        Content = new HtmlString("First")
                    });

                    bcContext.AddItem(new BreadcrumbsItem()
                    {
                        Href = "second",
                        Content = new HtmlString("Second")
                    });

                    bcContext.AddItem(new BreadcrumbsItem()
                    {
                        Content = new HtmlString("Last")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new BreadcrumbsTagHelper(new DefaultGovUkHtmlGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<div class=\"govuk-breadcrumbs\">" +
                "<ol class=\"govuk-breadcrumbs__list\">" +
                "<li class=\"govuk-breadcrumbs__list-item\"><a class=\"govuk-breadcrumbs__link\" href=\"first\">First</a></li>" +
                "<li class=\"govuk-breadcrumbs__list-item\"><a class=\"govuk-breadcrumbs__link\" href=\"second\">Second</a></li>" +
                "<li aria-current=\"page\" class=\"govuk-breadcrumbs__list-item\">Last</li>" +
                "</ol>" +
                "</div>",
                html);
        }
    }

    public class BreadcrumbsItemTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_NoLinkAddsItemToContext()
        {
            // Arrange
            var bcContext = new BreadcrumbsContext();

            var context = new TagHelperContext(
                tagName: "govuk-breadcrumbs-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(BreadcrumbsContext), bcContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-breadcrumbs-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent("The item");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new BreadcrumbsItemTagHelper(
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var lastItem = bcContext.Items.Last();
            Assert.Null(lastItem.Href);
            Assert.Equal("The item", lastItem.Content.AsString());
        }

        [Fact]
        public async Task ProcessAsync_WithLinkAddsItemToContext()
        {
            // Arrange
            var bcContext = new BreadcrumbsContext();

            var context = new TagHelperContext(
                tagName: "govuk-breadcrumbs-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(BreadcrumbsContext), bcContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-breadcrumbs-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent("The item");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new BreadcrumbsItemTagHelper(
                new DefaultGovUkHtmlGenerator(),
                Mock.Of<IUrlHelperFactory>())
            {
                Href = "place.com"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var lastItem = bcContext.Items.Last();
            Assert.Equal("place.com", lastItem.Href);
            Assert.Equal("The item", lastItem.Content.AsString());
        }
    }
}
