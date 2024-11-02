using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TextAreaValueTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsValueOnContext()
    {
        // Arrange
        var textAreaContext = new TextAreaContext();

        var context = new TagHelperContext(
            tagName: "govuk-textarea-value",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>() { { typeof(TextAreaContext), textAreaContext } },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-textarea-value",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Value");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new TextAreaValueTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("Value", textAreaContext.Value?.ToString());
    }
}
