using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class WarningTextTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var iconFallbackText = "fallback text";
        var classes = "custom-class";
        var dataFooAttrValue = "bar";
        var content = "My content";

        var context = new TagHelperContext(
            tagName: "govuk-warning-text",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-warning-text",
            attributes: new TagHelperAttributeList()
            {
                { "class", classes },
                { "data-foo", dataFooAttrValue },
            },
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(content);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        WarningTextOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateWarningText(It.IsAny<WarningTextOptions>())).Callback<WarningTextOptions>(o => actualOptions = o);

        var tagHelper = new WarningTextTagHelper(componentGeneratorMock.Object)
        {
            IconFallbackText = iconFallbackText
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(HtmlEncoder.Default.Encode(content), actualOptions!.Html);
        Assert.Null(actualOptions.Text);
        Assert.Equal(iconFallbackText, actualOptions.IconFallbackText);
        Assert.Collection(actualOptions.Attributes, kvp =>
        {
            Assert.Equal("data-foo", kvp.Key);
            Assert.Equal(dataFooAttrValue, kvp.Value);
        });
    }
}
