using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ErrorMessageTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var id = "my-error-message";
        var visuallyHiddenText = "vht";
        var classes = "custom-class";
        var dataFooAttrValue = "bar";
        var content = "Error content";

        var context = new TagHelperContext(
            tagName: "govuk-error-message",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-message",
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
        ErrorMessageOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateErrorMessage(It.IsAny<ErrorMessageOptions>())).Callback<ErrorMessageOptions>(o => actualOptions = o);

        var tagHelper = new ErrorMessageTagHelper(componentGeneratorMock.Object)
        {
            Id = id,
            VisuallyHiddenText = visuallyHiddenText
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(HtmlEncoder.Default.Encode(content), actualOptions!.Html);
        Assert.Null(actualOptions.Text);
        Assert.Equal(id, actualOptions.Id);
        Assert.Equal(visuallyHiddenText, actualOptions.VisuallyHiddenText);
        Assert.Equal(classes, actualOptions.Classes);
        Assert.NotNull(actualOptions.Attributes);
        Assert.Collection(actualOptions.Attributes, kvp =>
        {
            Assert.Equal("data-foo", kvp.Key);
            Assert.Equal(dataFooAttrValue, kvp.Value);
        });
    }

    [Fact]
    public async Task ProcessAsync_AspForSpecified_GeneratesExpectedHtml()
    {
        // Arrange
        var modelStateError = "An error!";

        var context = new TagHelperContext(
            tagName: "govuk-error-message",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-message",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
        output.TagMode = TagMode.SelfClosing;

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetValidationMessage(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateError);

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.SimpleProperty));

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        ErrorMessageOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateErrorMessage(It.IsAny<ErrorMessageOptions>())).Callback<ErrorMessageOptions>(o => actualOptions = o);

        var tagHelper = new ErrorMessageTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            For = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(HtmlEncoder.Default.Encode(modelStateError), actualOptions?.Html);
    }

    [Fact]
    public async Task ProcessAsync_AspForSpecifiedButNoError_GeneratesNoOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-error-message",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-message",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
        output.TagMode = TagMode.SelfClosing;

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetValidationMessage(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns((string?)null);

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.SimpleProperty));

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };

        var tagHelper = new ErrorMessageTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            For = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Null(output.TagName);
    }

    [Fact]
    public async Task ProcessAsync_NoAspForOrContent_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-error-message",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-error-message",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
        output.TagMode = TagMode.SelfClosing;

        var modelHelperMock = new Mock<IModelHelper>();

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };

        var tagHelper = new ErrorMessageTagHelper(componentGeneratorMock.Object, modelHelperMock.Object);

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Cannot determine content. Element must contain content if the 'asp-for' attribute is not specified.", ex.Message);
    }

    private class Model
    {
        public string? SimpleProperty { get; set; }
    }
}
