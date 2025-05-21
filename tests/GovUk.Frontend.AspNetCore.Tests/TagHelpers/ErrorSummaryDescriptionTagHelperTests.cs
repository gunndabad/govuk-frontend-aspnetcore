using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ErrorSummaryDescriptionTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsDescriptionToContext()
    {
        // Arrange
        var descriptionContent = "Description content";

        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-description",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ErrorSummaryContext), errorSummaryContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-summary-description",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(descriptionContent);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new ErrorSummaryDescriptionTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(HtmlEncoder.Default.Encode(descriptionContent), errorSummaryContext.Description?.Html);
    }
}
