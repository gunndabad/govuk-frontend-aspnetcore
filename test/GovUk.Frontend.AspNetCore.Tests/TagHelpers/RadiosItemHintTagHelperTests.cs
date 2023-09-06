using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class RadiosItemHintTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsHintOnContext()
    {
        // Arrange
        var radiosItemContext = new RadiosItemContext();

        var context = new TagHelperContext(
            tagName: "govuk-radios-item-hint",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosItemContext), radiosItemContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item-hint",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Hint");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemHintTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("Hint", radiosItemContext.Hint?.Content?.ToHtmlString());
    }
}
