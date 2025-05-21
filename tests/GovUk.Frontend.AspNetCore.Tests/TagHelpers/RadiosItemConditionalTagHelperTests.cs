using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class RadiosItemConditionalTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsConditionalOnContext()
    {
        // Arrange
        var radiosItemContext = new RadiosItemContext();

        var context = new TagHelperContext(
            tagName: "govuk-radios-item-Conditional",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(RadiosItemContext), radiosItemContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-radios-item-Conditional",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Conditional");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new RadiosItemConditionalTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("Conditional", radiosItemContext.Conditional?.Content?.ToHtmlString());
    }
}
