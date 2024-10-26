using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class BackLinkTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var href = "http://foo.com";
        var classes = "custom-class";
        var dataFooAttrValue = "bar";
        var content = "My custom link content";

        var context = new TagHelperContext(
            tagName: "govuk-back-link",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-back-link",
            attributes: new TagHelperAttributeList()
            {
                { "href", href },
                { "class", classes },
                { "data-foo", dataFooAttrValue }
            },
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(content);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        BackLinkOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateBackLink(It.IsAny<BackLinkOptions>())).Callback<BackLinkOptions>(o => actualOptions = o);

        var tagHelper = new BackLinkTagHelper(componentGeneratorMock.Object);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(HtmlEncoder.Default.Encode(content), actualOptions!.Html);
        Assert.Null(actualOptions.Text);
        Assert.Equal(href, actualOptions.Href);
        Assert.Equal(classes, actualOptions.Classes);
        Assert.NotNull(actualOptions.Attributes);
        Assert.Collection(actualOptions.Attributes, kvp =>
        {
            Assert.Equal("data-foo", kvp.Key);
            Assert.Equal(dataFooAttrValue, kvp.Value);
        });
    }
}
