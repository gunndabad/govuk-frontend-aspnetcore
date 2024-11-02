using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ErrorSummaryTitleTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsTitleToContext()
    {
        // Arrange
        var titleHtml = "Title content";

        var errorSummaryContext = new ErrorSummaryContext();

        var context = new TagHelperContext(
            tagName: "govuk-error-summary-title",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>() { { typeof(ErrorSummaryContext), errorSummaryContext } },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-error-summary-title",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(titleHtml);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new ErrorSummaryTitleTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(titleHtml, errorSummaryContext?.Title?.Html);
    }
}
