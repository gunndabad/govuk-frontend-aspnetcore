using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class InsetTextTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var content = "Inset text";
        var id = "id";

        var context = new TagHelperContext(
            tagName: "govuk-inset-text",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-inset-text",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(content);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InsetTextOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateInsetTextAsync(It.IsAny<InsetTextOptions>())).Callback<InsetTextOptions>(o => actualOptions = o);

        var tagHelper = new InsetTextTagHelper(componentGeneratorMock.Object, HtmlEncoder.Default)
        {
            Id = id
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(content, actualOptions!.Html);
        Assert.Null(actualOptions.Text);
        Assert.Equal(id, actualOptions.Id);
    }
}
