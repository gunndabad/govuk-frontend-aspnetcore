using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ButtonLinkTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var id = "my-button";
        var isStartButton = true;
        var href = "http://foo.com";
        var classes = "custom-class";
        var dataFooAttrValue = "bar";
        var content = "Button text";

        var context = new TagHelperContext(
            tagName: "govuk-button-link",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-button-link",
            attributes: new TagHelperAttributeList()
            {
                { "href", href },
                { "class", classes },
                { "data-foo", dataFooAttrValue },
            },
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(content);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<LegacyComponentGenerator>() { CallBase = true };
        ButtonOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateButton(It.IsAny<ButtonOptions>())).Callback<ButtonOptions>(o => actualOptions = o);

        var tagHelper = new ButtonLinkTagHelper(componentGeneratorMock.Object)
        {
            Id = id,
            IsStartButton = isStartButton,
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal("a", actualOptions!.Element);
        Assert.Equal(content, actualOptions.Html?.ToHtmlString());
        Assert.Null(actualOptions.Text);
        Assert.Null(actualOptions.Name);
        Assert.Null(actualOptions.Type);
        Assert.Null(actualOptions.Value);
        Assert.Null(actualOptions.Disabled);
        Assert.Equal(href, actualOptions.Href?.ToHtmlString());
        Assert.Equal(classes, actualOptions.Classes?.ToHtmlString());
        Assert.NotNull(actualOptions.Attributes);
        Assert.Contains(actualOptions.Attributes, kvp => kvp.Key == "data-foo" && kvp.Value == dataFooAttrValue);
        Assert.Null(actualOptions.PreventDoubleClick);
        Assert.Equal(isStartButton, actualOptions.IsStartButton);
        Assert.Equal(id, actualOptions.Id?.ToHtmlString());
    }
}
