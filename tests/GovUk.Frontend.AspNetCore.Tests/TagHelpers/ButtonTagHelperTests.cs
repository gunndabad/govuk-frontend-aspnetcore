using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ButtonTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var disabled = true;
        var id = "my-button";
        var isStartButton = true;
        var name = "MyButton";
        var preventDoubleClick = true;
        var type = "button";
        var value = "Value";
        var classes = "custom-class";
        var dataFooAttrValue = "bar";
        var content = "Button text";

        var context = new TagHelperContext(
            tagName: "govuk-button",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-button",
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
        ButtonOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateButtonAsync(It.IsAny<ButtonOptions>())).Callback<ButtonOptions>(o => actualOptions = o);

        var tagHelper = new ButtonTagHelper(componentGeneratorMock.Object)
        {
            Disabled = disabled,
            Id = id,
            IsStartButton = isStartButton,
            Name = name,
            PreventDoubleClick = preventDoubleClick,
            Type = type,
            Value = value
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal("button", actualOptions!.Element);
        Assert.Equal(HtmlEncoder.Default.Encode(content), actualOptions.Html);
        Assert.Null(actualOptions.Text);
        Assert.Equal(name, actualOptions.Name);
        Assert.Equal(type, actualOptions.Type);
        Assert.Equal(value, actualOptions.Value);
        Assert.Equal(disabled, actualOptions.Disabled);
        Assert.Null(actualOptions.Href);
        Assert.Equal(classes, actualOptions.Classes);
        Assert.NotNull(actualOptions.Attributes);
        Assert.Contains(actualOptions.Attributes, kvp => kvp.Key == "data-foo" && kvp.Value == dataFooAttrValue);
        Assert.Equal(preventDoubleClick, actualOptions.PreventDoubleClick);
        Assert.Equal(isStartButton, actualOptions.IsStartButton);
        Assert.Equal(id, actualOptions.Id);
    }
}
