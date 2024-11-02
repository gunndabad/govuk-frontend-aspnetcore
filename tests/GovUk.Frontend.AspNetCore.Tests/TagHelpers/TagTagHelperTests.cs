using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TagTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var html = "A tag";

        var context = new TagHelperContext(
            tagName: "govuk-tag",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-tag",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(html);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        TagOptions? actualOptions = null;
        componentGeneratorMock
            .Setup(mock => mock.GenerateTag(It.IsAny<TagOptions>()))
            .Callback<TagOptions>(o => actualOptions = o);

        var tagHelper = new TagTagHelper(componentGeneratorMock.Object);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(html, actualOptions!.Html);
    }
}
