using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class BreadcrumbsItemTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_NoLink_AddsItemToContext()
        {
            // Arrange
            var breadcrumbsContext = new BreadcrumbsContext();

            var context = new TagHelperContext(
                tagName: "govuk-breadcrumbs-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(BreadcrumbsContext), breadcrumbsContext }
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
                new ComponentGenerator(),
                Mock.Of<IUrlHelperFactory>());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var lastItem = breadcrumbsContext.Items.Last();
            Assert.Null(lastItem.Href);
            Assert.Equal("The item", lastItem.Content.RenderToString());
        }

        [Fact]
        public async Task ProcessAsync_WithLink_AddsItemToContext()
        {
            // Arrange
            var breadcrumbsContext = new BreadcrumbsContext();

            var context = new TagHelperContext(
                tagName: "govuk-breadcrumbs-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(BreadcrumbsContext), breadcrumbsContext }
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

            var tagHelper = new BreadcrumbsItemTagHelper(Mock.Of<IUrlHelperFactory>())
            {
                Href = "place.com"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var lastItem = breadcrumbsContext.Items.Last();
            Assert.Equal("place.com", lastItem.Href);
            Assert.Equal("The item", lastItem.Content.RenderToString());
        }
    }
}
