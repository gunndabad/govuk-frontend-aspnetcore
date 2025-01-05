using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class BreadcrumbsItemTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_NoLink_AddsItemToContext()
    {
        // Arrange
        var content = "The item";

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
                tagHelperContent.SetHtmlContent(content);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new BreadcrumbsItemTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var lastItem = breadcrumbsContext.Items.Last();
        Assert.Null(lastItem.Href);
        Assert.Equal(content, lastItem.Html?.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithLink_AddsItemToContext()
    {
        // Arrange
        var content = "The item";
        var href = "http://place.com";

        var breadcrumbsContext = new BreadcrumbsContext();

        var context = new TagHelperContext(
            tagName: "govuk-breadcrumbs-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(BreadcrumbsContext), breadcrumbsContext }
            },
            uniqueId: "test");

        var attributes = new TagHelperAttributeList();
        var output = new TagHelperOutput(
            "govuk-breadcrumbs-item",
            attributes,
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                attributes.Add("href", href);

                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetHtmlContent(content);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new BreadcrumbsItemTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var lastItem = breadcrumbsContext.Items.Last();
        Assert.Equal(href, lastItem.Href?.ToHtmlString());
        Assert.Equal(content, lastItem.Html?.ToHtmlString());
    }
}
