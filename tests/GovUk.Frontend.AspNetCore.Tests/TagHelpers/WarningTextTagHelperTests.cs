using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class WarningTextTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
        tagName: "govuk-warning-text",
        allAttributes: new TagHelperAttributeList(),
        items: new Dictionary<object, object>(),
        uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-warning-text",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Warning message");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new WarningTextTagHelper(new ComponentGenerator())
        {
            IconFallbackText = "Danger"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-warning-text"">
    <span aria-hidden=""true"" class=""govuk-warning-text__icon"">!</span>
        <strong class=""govuk-warning-text__text"">
        <span class=""govuk-visually-hidden"">Danger</span>
        Warning message
    </strong>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }
}
