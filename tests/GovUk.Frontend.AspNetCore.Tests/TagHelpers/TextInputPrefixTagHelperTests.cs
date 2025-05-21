using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TextInputPrefixTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsPrefixOnContext()
    {
        // Arrange
        var prefix = "Prefix";
        var inputContext = new TextInputContext();

        var context = new TagHelperContext(
            tagName: "govuk-input-prefix",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(TextInputContext), inputContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input-prefix",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.AppendHtml(new HtmlString(prefix));
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new TextInputPrefixTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(inputContext.Prefix);
        Assert.Equal(prefix, inputContext.Prefix?.Html?.ToHtmlString());
    }
}
