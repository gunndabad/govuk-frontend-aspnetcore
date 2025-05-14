using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TextInputSuffixTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsSuffixOnContext()
    {
        // Arrange
        var suffix = "Suffix";
        var inputContext = new TextInputContext();

        var context = new TagHelperContext(
            tagName: "govuk-input-suffix",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(TextInputContext), inputContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input-suffix",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.AppendHtml(new HtmlString(suffix));
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new TextInputSuffixTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(inputContext.Suffix);
        Assert.Equal(suffix, inputContext.Suffix!.Html);
    }
}
