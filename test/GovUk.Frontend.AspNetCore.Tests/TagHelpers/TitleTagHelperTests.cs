using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TitleTagHelperTests
{
    [Theory]
    [InlineData(false, false, false)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(true, true, true)]
    public async Task ProcessAsync_GeneratesExpectedOutput(
        bool prependErrorToTitleOption,
        bool modelStateHasErrors,
        bool expectErrorInTitle)
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions()
        {
            PrependErrorToTitle = prependErrorToTitleOption
        });

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

        if (modelStateHasErrors)
        {
            viewContext.ModelState.AddModelError("Key", "An error");
        }

        var tagHelper = new TitleTagHelper(options)
        {
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
