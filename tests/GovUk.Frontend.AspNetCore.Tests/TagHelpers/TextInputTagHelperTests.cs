using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TextInputTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var id = "my-id";
        var describedBy = "describedby";
        var name = "my-name";
        var autocomplete = "none";
        var inputMode = "numeric";
        var pattern = "[0-9]*";
        var spellcheck = false;
        var type = "number";
        var value = "42";
        var disabled = true;
        var labelClass = "additional-label-class";
        var classes = "custom-class";
        var dataFooAttrValue = "foo";
        var labelHtml = "The label";
        var hintHtml = "The hint";

        var context = new TagHelperContext(
            tagName: "govuk-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var inputContext = context.GetContextItem<TextInputContext>();

                inputContext.SetLabel(
                    isPageHeading: false,
                    attributes: new AttributeCollection(),
                    new HtmlString(labelHtml),
                    TextInputTagHelper.LabelTagName);

                inputContext.SetHint(
                    attributes: new AttributeCollection(),
                    new HtmlString(hintHtml),
                    TextInputTagHelper.HintTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateInputAsync(It.IsAny<InputOptions>())).Callback<InputOptions>(o => actualOptions = o);

        var tagHelper = new TextInputTagHelper(componentGeneratorMock.Object, modelHelperMock.Object, HtmlEncoder.Default)
        {
            Id = id,
            DescribedBy = describedBy,
            Name = name,
            Autocomplete = autocomplete,
            InputMode = inputMode,
            Pattern = pattern,
            Spellcheck = spellcheck,
            Type = type,
            Value = value,
            Disabled = disabled,
            LabelClass = labelClass,
            ViewContext = new ViewContext(),
            InputAttributes = new Dictionary<string, string?>()
            {
                { "class", classes },
                { "data-foo", dataFooAttrValue },
            }
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(id, actualOptions.Id);
        Assert.Equal(name, actualOptions.Name);
        Assert.Equal(type, actualOptions.Type);
        Assert.Equal(inputMode, actualOptions.InputMode);
        Assert.Equal(value, actualOptions.Value);
        Assert.Equal(disabled, actualOptions.Disabled);
        Assert.Equal(describedBy, actualOptions.DescribedBy);
        Assert.Equal(labelHtml, actualOptions.Label?.Html);
        Assert.Equal(hintHtml, actualOptions.Hint?.Html);
        Assert.Null(actualOptions.ErrorMessage);
        Assert.Equal(classes, actualOptions.Classes);
        Assert.Equal(autocomplete, actualOptions.AutoComplete);
        Assert.Equal(pattern, actualOptions.Pattern);
        Assert.Equal(spellcheck, actualOptions.Spellcheck);

        Assert.NotNull(actualOptions.Attributes);
        Assert.Collection(actualOptions.Attributes, kvp =>
        {
            Assert.Equal("data-foo", kvp.Key);
            Assert.Equal(dataFooAttrValue, kvp.Value);
        });
    }

    [Fact]
    public async Task ProcessAsync_WithErrorMessage_GeneratesOptionsWithErrorMessage()
    {
        // Arrange
        var id = "my-id";
        var name = "my-name";
        var labelHtml = "The label";
        var errorHtml = "The error message";
        var errorVht = "visually hidden text";
        var errorDataFooAttribute = "bar";

        var context = new TagHelperContext(
            tagName: "govuk-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var inputContext = context.GetContextItem<TextInputContext>();

                inputContext.SetLabel(
                    isPageHeading: false,
                    new AttributeCollection(),
                    new HtmlString(labelHtml),
                    TextInputTagHelper.LabelTagName);

                inputContext.SetErrorMessage(
                    visuallyHiddenText: new HtmlString(errorVht),
                    attributes: new AttributeCollection()
                    {
                        { "data-foo", errorDataFooAttribute }
                    },
                    new HtmlString(errorHtml),
                    TextInputTagHelper.ErrorMessageTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateInputAsync(It.IsAny<InputOptions>())).Callback<InputOptions>(o => actualOptions = o);

        var tagHelper = new TextInputTagHelper(componentGeneratorMock.Object, modelHelperMock.Object, HtmlEncoder.Default)
        {
            Id = id,
            Name = name,
            ViewContext = TestUtils.CreateViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions?.ErrorMessage);
        Assert.Equal(errorHtml, actualOptions.ErrorMessage.Html);
        Assert.Equal(errorVht, actualOptions.ErrorMessage.VisuallyHiddenText);
        Assert.NotNull(actualOptions.ErrorMessage.Attributes);
        Assert.Collection(actualOptions.ErrorMessage.Attributes, kvp =>
        {
            Assert.Equal("data-foo", kvp.Key);
            Assert.Equal(errorDataFooAttribute, kvp.Value);
        });
    }

    [Fact]
    public async Task ProcessAsync_WithFor_GeneratesOptionsFromModelMetadata()
    {
        // Arrange
        var modelStateValue = "42";
        var displayName = "The label";
        var description = "The hint";
        var modelStateError = "The error message";

        var context = new TagHelperContext(
            tagName: "govuk-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.SimpleProperty));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(displayName);

        modelHelperMock
            .Setup(mock => mock.GetDescription(/*modelExplorer: */It.IsAny<ModelExplorer>()))
            .Returns(description);

        modelHelperMock
            .Setup(mock => mock.GetValidationMessage(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateError);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateValue);

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.SimpleProperty));

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateInputAsync(It.IsAny<InputOptions>())).Callback<InputOptions>(o => actualOptions = o);

        var tagHelper = new TextInputTagHelper(componentGeneratorMock.Object, modelHelperMock.Object, HtmlEncoder.Default)
        {
            For = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = TestUtils.CreateViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.NotNull(actualOptions.Id);
        Assert.NotNull(actualOptions.Name);
        Assert.Equal(modelStateValue, actualOptions.Value);
        Assert.Equal(displayName, actualOptions.Label?.Html);
        Assert.Equal(description, actualOptions.Hint?.Html);
        Assert.Equal(modelStateError, actualOptions.ErrorMessage?.Html);
    }

    [Fact]
    public async Task ProcessAsync_WithForAndExplicitValue_UsesSpecifiedValue()
    {
        // Arrange
        var modelStateValue = "42";
        var displayName = "The label";
        var value = "Explicit value";

        var context = new TagHelperContext(
            tagName: "govuk-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.SimpleProperty));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(displayName);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateValue);

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.SimpleProperty));

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateInputAsync(It.IsAny<InputOptions>())).Callback<InputOptions>(o => actualOptions = o);

        var tagHelper = new TextInputTagHelper(componentGeneratorMock.Object, modelHelperMock.Object, HtmlEncoder.Default)
        {
            For = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext(),
            Value = value
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(value, actualOptions?.Value);
    }

    [Fact]
    public async Task ProcessAsync_WithForAndExplicitLabel_UsesSpecifiedLabel()
    {
        // Arrange
        var modelStateValue = "42";
        var modelStateDisplayName = "ModelState label";
        var labelHtml = "Explicit label";

        var context = new TagHelperContext(
            tagName: "govuk-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var inputContext = context.GetContextItem<TextInputContext>();

                inputContext.SetLabel(
                    isPageHeading: false,
                    new AttributeCollection(),
                    new HtmlString(labelHtml),
                    TextInputTagHelper.LabelTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.SimpleProperty));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateDisplayName);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateValue);

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.SimpleProperty));

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateInputAsync(It.IsAny<InputOptions>())).Callback<InputOptions>(o => actualOptions = o);

        var tagHelper = new TextInputTagHelper(componentGeneratorMock.Object, modelHelperMock.Object, HtmlEncoder.Default)
        {
            For = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(labelHtml, actualOptions?.Label?.Html);
    }

    [Fact]
    public async Task ProcessAsync_WithForAndExplicitHint_UsesSpecifiedHint()
    {
        // Arrange
        var modelStateValue = "42";
        var displayName = "The label";
        var modelStateDescription = "The hint";
        var hintHtml = "Explicit hint";

        var context = new TagHelperContext(
            tagName: "govuk-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var inputContext = context.GetContextItem<TextInputContext>();

                inputContext.SetHint(new AttributeCollection(), new HtmlString(hintHtml), TextInputTagHelper.HintTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.SimpleProperty));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(displayName);

        modelHelperMock
            .Setup(mock => mock.GetDescription(/*modelExplorer: */It.IsAny<ModelExplorer>()))
            .Returns(modelStateDescription);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateValue);

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.SimpleProperty));

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateInputAsync(It.IsAny<InputOptions>())).Callback<InputOptions>(o => actualOptions = o);

        var tagHelper = new TextInputTagHelper(componentGeneratorMock.Object, modelHelperMock.Object, HtmlEncoder.Default)
        {
            For = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(hintHtml, actualOptions?.Hint?.Html);
    }

    [Fact]
    public async Task ProcessAsync_WithForAndExplicitErrorMessage_UsesSpecifiedErrorMessage()
    {
        // Arrange
        var modelStateValue = "42";
        var displayName = "The label";
        var modelStateError = "ModelState error";
        var errorHtml = "Explicit error";

        var context = new TagHelperContext(
            tagName: "govuk-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var inputContext = context.GetContextItem<TextInputContext>();

                inputContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    new AttributeCollection(),
                    new HtmlString(errorHtml),
                    TextInputTagHelper.ErrorMessageTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.SimpleProperty));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(displayName);

        modelHelperMock
            .Setup(mock => mock.GetValidationMessage(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateError);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateValue);

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.SimpleProperty));

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateInputAsync(It.IsAny<InputOptions>())).Callback<InputOptions>(o => actualOptions = o);

        var tagHelper = new TextInputTagHelper(componentGeneratorMock.Object, modelHelperMock.Object, HtmlEncoder.Default)
        {
            For = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = TestUtils.CreateViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(errorHtml, actualOptions?.ErrorMessage?.Html);
    }

    [Fact]
    public async Task ProcessAsync_WithForAndNoExplicitErrorMessageAndIgnoreModelStateErrorTrue_DoesNotRenderErrorMessage()
    {
        // Arrange
        var modelStateValue = "42";
        var displayName = "The label";
        var modelStateError = "ModelState error";

        var context = new TagHelperContext(
            tagName: "govuk-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.SimpleProperty));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(displayName);

        modelHelperMock
            .Setup(mock => mock.GetValidationMessage(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateError);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateValue);

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.SimpleProperty));

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateInputAsync(It.IsAny<InputOptions>())).Callback<InputOptions>(o => actualOptions = o);

        var tagHelper = new TextInputTagHelper(componentGeneratorMock.Object, modelHelperMock.Object, HtmlEncoder.Default)
        {
            For = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext(),
            IgnoreModelStateErrors = true
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Null(actualOptions?.ErrorMessage);
    }

    [Fact]
    public async Task ProcessAsync_WithForAndAndExplicitErrorMessageAndIgnoreModelStateErrorTrue_DoesRenderErrorMessage()
    {
        // Arrange
        var modelStateValue = "42";
        var displayName = "The label";
        var modelStateError = "ModelState error";
        var errorHtml = "Explicit error";

        var context = new TagHelperContext(
            tagName: "govuk-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var inputContext = context.GetContextItem<TextInputContext>();

                inputContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    new AttributeCollection(),
                    new HtmlString(errorHtml),
                    TextInputTagHelper.ErrorMessageTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        modelHelperMock
            .Setup(mock => mock.GetFullHtmlFieldName(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*expression: */It.IsAny<string>()))
            .Returns(nameof(Model.SimpleProperty));

        modelHelperMock
            .Setup(mock => mock.GetDisplayName(
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(displayName);

        modelHelperMock
            .Setup(mock => mock.GetValidationMessage(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateError);

        modelHelperMock
            .Setup(mock => mock.GetModelValue(
                /*viewContext: */It.IsAny<ViewContext>(),
                /*modelExplorer: */It.IsAny<ModelExplorer>(),
                /*expression: */It.IsAny<string>()))
            .Returns(modelStateValue);

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
            .GetExplorerForProperty(nameof(Model.SimpleProperty));

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        InputOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateInputAsync(It.IsAny<InputOptions>())).Callback<InputOptions>(o => actualOptions = o);

        var tagHelper = new TextInputTagHelper(componentGeneratorMock.Object, modelHelperMock.Object, HtmlEncoder.Default)
        {
            For = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            IgnoreModelStateErrors = true,
            ViewContext = TestUtils.CreateViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(errorHtml, actualOptions?.ErrorMessage?.Html);
    }

    [Fact]
    public async Task ProcessAsync_WithError_AddsErrorWithCorrectFieldIdToContainerErrorContext()
    {
        // Arrange
        var id = "my-id";
        var name = "my-name";
        var labelHtml = "The label";
        var errorHtml = "The error message";

        var context = new TagHelperContext(
            tagName: "govuk-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-input",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var inputContext = context.GetContextItem<TextInputContext>();

                inputContext.SetLabel(
                    isPageHeading: false,
                    new AttributeCollection(),
                    new HtmlString(labelHtml),
                    TextInputTagHelper.LabelTagName);

                inputContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    new AttributeCollection(),
                    new HtmlString(errorHtml),
                    TextInputTagHelper.ErrorMessageTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };

        var tagHelper = new TextInputTagHelper(componentGeneratorMock.Object, modelHelperMock.Object, HtmlEncoder.Default)
        {
            Id = id,
            Name = name,
            ViewContext = TestUtils.CreateViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            tagHelper.ViewContext.HttpContext.GetContainerErrorContext().Errors,
            error =>
            {
                Assert.Equal(errorHtml, error.Html);
                Assert.Equal($"#{id}", error.Href);
            });
    }

    private class Model
    {
        public string? SimpleProperty { get; set; }
    }
}
