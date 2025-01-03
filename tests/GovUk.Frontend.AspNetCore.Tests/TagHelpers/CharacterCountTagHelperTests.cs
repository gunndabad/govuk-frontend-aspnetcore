using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class CharacterCountTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var id = "my-id";
        var name = "my-name";
        var autocomplete = "none";
        var rows = 6;
        var spellcheck = false;
        var value = "42";
        var disabled = true;
        var threshold = 89m;
        var maxLength = 42;
        var labelClass = "additional-label-class";
        var classes = "custom-class";
        var dataFooAttrValue = "bar";
        var formGroupClasses = "formgroup-class";
        var formGroupDataBarAttribute = "baz";
        var countMessageClasses = "countmessage-class";
        var countMessageDataBazAttribute = "baz";
        var labelHtml = "The label";
        var hintHtml = "The hint";

        var context = new TagHelperContext(
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var characterCountContext = context.GetContextItem<CharacterCountContext>();

                characterCountContext.SetLabel(
                    isPageHeading: false,
                    attributes: new EncodedAttributesDictionary(),
                    new HtmlString(labelHtml),
                    CharacterCountTagHelper.LabelTagName);

                characterCountContext.SetHint(
                    attributes: new EncodedAttributesDictionary(),
                    new HtmlString(hintHtml),
                    CharacterCountTagHelper.HintTagName);

                characterCountContext.SetValue(new HtmlString(value), CharacterCountValueTagHelper.TagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            Id = id,
            Name = name,
            Rows = rows,
            Autocomplete = autocomplete,
            CountMessageAttributes = new Dictionary<string, string?>()
            {
                { "class", countMessageClasses },
                { "data-baz", countMessageDataBazAttribute }
            },
            Spellcheck = spellcheck,
            Disabled = disabled,
            FormGroupAttributes = new Dictionary<string, string?>()
            {
                { "class", formGroupClasses },
                { "data-bar", formGroupDataBarAttribute }
            },
            LabelClass = labelClass,
            MaxLength = maxLength,
            TextAreaAttributes = new Dictionary<string, string?>()
            {
                { "class", classes },
                { "data-foo", dataFooAttrValue }
            },
            Threshold = threshold
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(id, actualOptions.Id?.ToHtmlString());
        Assert.Equal(name, actualOptions.Name?.ToHtmlString());
        Assert.Equal(rows, actualOptions.Rows);
        Assert.Equal(value, actualOptions.Value?.ToHtmlString());
        Assert.Equal(maxLength, actualOptions.MaxLength);
        Assert.Equal(threshold, actualOptions.Threshold);
        Assert.Equal(labelHtml, actualOptions.Label?.Html?.ToHtmlString());
        Assert.Equal(hintHtml, actualOptions.Hint?.Html?.ToHtmlString());
        Assert.Null(actualOptions.ErrorMessage);
        Assert.Equal(classes, actualOptions.Classes?.ToHtmlString());
        Assert.Equal(spellcheck, actualOptions.Spellcheck);

        Assert.NotNull(actualOptions.Attributes);
        Assert.Contains(actualOptions.Attributes, kvp => kvp.Key == "autocomplete" && kvp.Value == autocomplete);
        Assert.Contains(actualOptions.Attributes, kvp => kvp.Key == "data-foo" && kvp.Value == dataFooAttrValue);
        Assert.Contains(actualOptions.Attributes, kvp => kvp.Key == "disabled");

        Assert.NotNull(actualOptions.CountMessage);
        Assert.Equal(countMessageClasses, actualOptions.CountMessage.Classes?.ToHtmlString());
        Assert.NotNull(actualOptions.CountMessage.Attributes);
        Assert.Contains(actualOptions.CountMessage.Attributes, kvp => kvp.Key == "data-baz" && kvp.Value == countMessageDataBazAttribute);

        Assert.NotNull(actualOptions.FormGroup);
        Assert.Equal(formGroupClasses, actualOptions.FormGroup.Classes?.ToHtmlString());
        Assert.NotNull(actualOptions.FormGroup.Attributes);
        Assert.Contains(actualOptions.FormGroup.Attributes, kvp => kvp.Key == "data-bar" && kvp.Value == formGroupDataBarAttribute);
    }

    [Fact]
    public async Task ProcessAsync_WithMaxWords_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var id = "my-id";
        var name = "my-name";
        var maxWords = 42;
        var labelHtml = "The label";

        var context = new TagHelperContext(
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var characterCountContext = context.GetContextItem<CharacterCountContext>();

                characterCountContext.SetLabel(
                    isPageHeading: false,
                    attributes: new EncodedAttributesDictionary(),
                    new HtmlString(labelHtml),
                    CharacterCountTagHelper.LabelTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            Id = id,
            Name = name,
            MaxWords = maxWords
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(maxWords, actualOptions.MaxWords);
    }

    [Fact]
    public async Task ProcessAsync_WithErrorMessage_GeneratesOptionsWithErrorMessageAndAddsErrorClasses()
    {
        // Arrange
        var id = "my-id";
        var name = "my-name";
        var labelHtml = "The label";
        var errorHtml = "The error message";
        var errorVht = "visually hidden text";
        var errorDataFooAttribute = "bar";

        var context = new TagHelperContext(
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var characterCountContext = context.GetContextItem<CharacterCountContext>();

                characterCountContext.SetLabel(
                    isPageHeading: false,
                    new EncodedAttributesDictionary(),
                    new HtmlString(labelHtml),
                    CharacterCountTagHelper.LabelTagName);

                characterCountContext.SetErrorMessage(
                    visuallyHiddenText: new HtmlString(errorVht),
                    attributes: new EncodedAttributesDictionaryBuilder().With("data-foo", errorDataFooAttribute, encodeValue: false),
                    new HtmlString(errorHtml),
                    CharacterCountTagHelper.ErrorMessageTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            Id = id,
            Name = name,
            ViewContext = new ViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions?.ErrorMessage);
        Assert.Equal(errorHtml, actualOptions.ErrorMessage.Html?.ToHtmlString());
        Assert.Equal(errorVht, actualOptions.ErrorMessage.VisuallyHiddenText?.ToHtmlString());
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
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
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
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            AspFor = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.NotNull(actualOptions.Id);
        Assert.NotNull(actualOptions.Name);
        Assert.Equal(modelStateValue, actualOptions.Value?.ToHtmlString());
        Assert.Equal(displayName, actualOptions.Label?.Html?.ToHtmlString());
        Assert.Equal(description, actualOptions.Hint?.Html?.ToHtmlString());
        Assert.Equal(modelStateError, actualOptions.ErrorMessage?.Html?.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithForAndExplicitValue_UsesSpecifiedValue()
    {
        // Arrange
        var modelStateValue = "42";
        var modelStateDisplayName = "ModelState label";
        var value = "Explicit value";

        var context = new TagHelperContext(
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var characterCountContext = context.GetContextItem<CharacterCountContext>();

                characterCountContext.SetValue(new HtmlString(value), CharacterCountValueTagHelper.TagName);

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
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            AspFor = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(value, actualOptions?.Value?.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithForAndExplicitLabel_UsesSpecifiedLabel()
    {
        // Arrange
        var modelStateValue = "42";
        var modelStateDisplayName = "ModelState label";
        var labelHtml = "Explicit label";

        var context = new TagHelperContext(
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var characterCountContext = context.GetContextItem<CharacterCountContext>();

                characterCountContext.SetLabel(
                    isPageHeading: false,
                    new EncodedAttributesDictionary(),
                    new HtmlString(labelHtml),
                    CharacterCountTagHelper.LabelTagName);

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
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            AspFor = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(labelHtml, actualOptions?.Label?.Html?.ToHtmlString());
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
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var characterCountContext = context.GetContextItem<CharacterCountContext>();

                characterCountContext.SetHint(new EncodedAttributesDictionary(), new HtmlString(hintHtml), CharacterCountTagHelper.HintTagName);

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
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            AspFor = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(hintHtml, actualOptions?.Hint?.Html?.ToHtmlString());
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
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var characterCountContext = context.GetContextItem<CharacterCountContext>();

                characterCountContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    new EncodedAttributesDictionary(),
                    new HtmlString(errorHtml),
                    CharacterCountTagHelper.ErrorMessageTagName);

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
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            AspFor = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext(),
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(errorHtml, actualOptions?.ErrorMessage?.Html?.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithForAndNoExplicitErrorMessageAndIgnoreModelStateErrorTrue_DoesNotRenderErrorMessage()
    {
        // Arrange
        var modelStateValue = "42";
        var displayName = "The label";
        var modelStateError = "ModelState error";

        var context = new TagHelperContext(
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
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
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            AspFor = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
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
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var characterCountContext = context.GetContextItem<CharacterCountContext>();

                characterCountContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    new EncodedAttributesDictionary(),
                    new HtmlString(errorHtml),
                    CharacterCountTagHelper.ErrorMessageTagName);

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
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            AspFor = new ModelExpression(nameof(Model.SimpleProperty), modelExplorer),
            ViewContext = new ViewContext(),
            IgnoreModelStateErrors = true
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(errorHtml, actualOptions?.ErrorMessage?.Html?.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithError_AddsErrorWithCorrectFieldIdToContainerErrorContext()
    {
        // Arrange
        var containerErrorContext = new ContainerErrorContext();

        var id = "my-id";
        var name = "my-name";
        var labelHtml = "The label";
        var errorHtml = "The error message";

        var context = new TagHelperContext(
            tagName: "govuk-character-count",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(ContainerErrorContext), containerErrorContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-character-count",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var characterCountContext = context.GetContextItem<CharacterCountContext>();

                characterCountContext.SetLabel(
                    isPageHeading: false,
                    new EncodedAttributesDictionary(),
                    new HtmlString(labelHtml),
                    CharacterCountTagHelper.LabelTagName);

                characterCountContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    new EncodedAttributesDictionary(),
                    new HtmlString(errorHtml),
                    CharacterCountTagHelper.ErrorMessageTagName);

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var modelHelperMock = new Mock<IModelHelper>();

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        CharacterCountOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateCharacterCount(It.IsAny<CharacterCountOptions>())).Callback<CharacterCountOptions>(o => actualOptions = o);

        var tagHelper = new CharacterCountTagHelper(componentGeneratorMock.Object, modelHelperMock.Object)
        {
            Id = id,
            Name = name,
            ViewContext = new ViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            containerErrorContext.Errors,
            error =>
            {
                Assert.Equal(errorHtml, error.Content.ToHtmlString());
                Assert.Equal($"#{id}", error.Href?.ToHtmlString());
            });
    }

    private class Model
    {
        public string? SimpleProperty { get; set; }
    }
}
