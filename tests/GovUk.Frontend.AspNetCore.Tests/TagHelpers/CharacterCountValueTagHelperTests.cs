using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class CharacterCountValueTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsValueOnContext()
    {
        // Arrange
        var characterCountContext = new CharacterCountContext();

        var context = new TagHelperContext(
            tagName: "govuk-character-count-value",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(CharacterCountContext), characterCountContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count-value",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Value");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new CharacterCountValueTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("Value", characterCountContext.Value?.ToString());
    }
}
