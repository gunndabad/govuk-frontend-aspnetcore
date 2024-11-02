using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ErrorMessageTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsErrorMessageOnContext()
    {
        // Arrange
        var errorMessage = "Error message";
        var vht = "vht";

        var formGroupContext = new TestFormGroupContext();

        var context = new TagHelperContext(
            tagName: "test-error-message",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>() { { typeof(FormGroupContext2), formGroupContext } },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "test-error-message",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(errorMessage);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new ErrorMessageTagHelper() { VisuallyHiddenText = vht };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(formGroupContext.ErrorMessage);
        Assert.Equal(HtmlEncoder.Default.Encode(errorMessage), formGroupContext.ErrorMessage.Html);
        Assert.Equal(vht, formGroupContext.ErrorMessage.VisuallyHiddenText);
    }

    private class TestFormGroupContext : FormGroupContext2
    {
        protected override IReadOnlyCollection<string> ErrorMessageTagNames => [ShortTagNames.ErrorMessage];

        protected override IReadOnlyCollection<string> HintTagNames => [ShortTagNames.Hint];

        protected override IReadOnlyCollection<string> LabelTagNames => [ShortTagNames.Label];

        protected override string RootTagName => "test";
    }
}
