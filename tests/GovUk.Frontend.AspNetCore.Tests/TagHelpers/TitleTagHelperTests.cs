using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TitleTagHelperTests
{
    [Theory]
    [InlineData(false, false)]
    [InlineData(true, true)]
    public async Task ProcessAsync_GeneratesExpectedOutput(
        bool pageHasErrors,
        bool expectErrorInTitle)
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "form",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "form",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var viewContext = new ViewContext();
        viewContext.ViewData.SetPageHasErrors(pageHasErrors);

        var tagHelper = new TitleTagHelper()
        {
            ErrorPrefix = "Error:",
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var html = output.RenderToElement();
        var startsWithError = html.InnerHtml.StartsWith("Error: ");

        if (expectErrorInTitle)
        {
            Assert.True(startsWithError);
        }
        else
        {
            Assert.False(startsWithError);
        }
    }
}
