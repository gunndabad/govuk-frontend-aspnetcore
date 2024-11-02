using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class InsetTextTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var html = "Inset text";
        var id = "my-id";

        var context = new TagHelperContext(
            tagName: "govuk-inset-text",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-inset-text",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(html);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InsetTextOptions? actualOptions = null;
        componentGeneratorMock
            .Setup(mock => mock.GenerateInsetText(It.IsAny<InsetTextOptions>()))
            .Callback<InsetTextOptions>(o => actualOptions = o);

        var tagHelper = new InsetTextTagHelper(componentGeneratorMock.Object) { Id = id };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(id, actualOptions!.Id);
        Assert.Null(actualOptions.Text);
        Assert.Equal(html, actualOptions.Html);
    }
}
