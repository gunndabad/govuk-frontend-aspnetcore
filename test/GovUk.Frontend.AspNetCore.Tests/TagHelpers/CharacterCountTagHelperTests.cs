using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
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
    public async Task ProcessAsync_WithMaxWords_GeneratesExpectedOutput()
    {
        // Arrange
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
                    attributes: null,
                    content: new HtmlString("The label"));

                characterCountContext.SetHint(
                    attributes: null,
                    content: new HtmlString("The hint"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new CharacterCountTagHelper()
        {
            Id = "my-id",
            Name = "my-name",
            MaxWords = 10,
            Threshold = 90,
            LabelClass = "additional-label-class"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-character-count"" data-module=""govuk-character-count"" data-maxwords=""10"" data-threshold=""90"">
    <div class=""govuk-form-group"">
        <label class=""govuk-label additional-label-class"" for=""my-id"">The label</label>
        <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
        <textarea class=""govuk-textarea govuk-js-character-count"" id=""my-id"" name=""my-name"" rows=""5"" aria-describedby=""my-id-hint""></textarea>
    </div>
    <div id=""my-id-info"" class=""govuk-hint govuk-character-count__message"">
        You can enter up to 10 words
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithMaxLength_GeneratesExpectedOutput()
    {
        // Arrange
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
                    attributes: null,
                    content: new HtmlString("The label"));

                characterCountContext.SetHint(
                    attributes: null,
                    content: new HtmlString("The hint"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new CharacterCountTagHelper()
        {
            Id = "my-id",
            Name = "my-name",
            MaxLength = 200,
            Threshold = 90
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-character-count"" data-module=""govuk-character-count"" data-maxlength=""200"" data-threshold=""90"">
    <div class=""govuk-form-group"">
        <label class=""govuk-label"" for=""my-id"">The label</label>
        <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
        <textarea class=""govuk-textarea govuk-js-character-count"" id=""my-id"" name=""my-name"" rows=""5"" aria-describedby=""my-id-hint""></textarea>
    </div>
    <div id=""my-id-info"" class=""govuk-hint govuk-character-count__message"">
        You can enter up to 200 characters
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithError_GeneratesExpectedOutput()
    {
        // Arrange
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
                    attributes: null,
                    content: new HtmlString("The label"));

                characterCountContext.SetHint(
                    attributes: null,
                    content: new HtmlString("The hint"));

                characterCountContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    attributes: null,
                    content: new HtmlString("The error"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new CharacterCountTagHelper()
        {
            Id = "my-id",
            Name = "my-name",
            MaxWords = 10
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-character-count"" data-module=""govuk-character-count"" data-maxwords=""10"">
    <div class=""govuk-form-group govuk-form-group--error"">
        <label class=""govuk-label"" for=""my-id"">The label</label>
        <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
        <p id=""my-id-error"" class=""govuk-error-message"">
            <span class=""govuk-visually-hidden"">Error:</span>
            The error
        </p>
        <textarea class=""govuk-textarea govuk-js-character-count govuk-textarea--error"" id=""my-id"" name=""my-name"" rows=""5"" aria-describedby=""my-id-hint my-id-error""></textarea>
    </div>
    <div id=""my-id-info"" class=""govuk-hint govuk-character-count__message"">
        You can enter up to 10 words
    </div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_BothMaxLengthAndMaxWords_ThrowsInvalidOperationException()
    {
        // Arrange
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
                    attributes: null,
                    content: new HtmlString("The label"));

                characterCountContext.SetHint(
                    attributes: null,
                    content: new HtmlString("The hint"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new CharacterCountTagHelper()
        {
            Id = "my-id",
            Name = "my-name",
            MaxLength = 100,
            MaxWords = 10
        };

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one of the 'max-length' or 'max-words' attributes can be specified.", ex.Message);
    }

    [Fact]
    public async Task ProcessAsync_NoValueOrAspFor_RendersEmptyTextArea()
    {
        // Arrange
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
                    attributes: null,
                    content: new HtmlString("The label"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new CharacterCountTagHelper()
        {
            Name = "my-name",
            MaxWords = 10,
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();
        var textarea = element.GetElementsByTagName("textarea")[0];
        Assert.Empty(textarea.InnerHtml);
    }

    [Fact]
    public async Task ProcessAsync_WithAspFor_RendersTextAreaWithModelValue()
    {
        // Arrange
        var modelValue = "Foo value";
        var model = new Model()
        {
            Foo = modelValue
        };

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), model)
            .GetExplorerForProperty(nameof(Model.Foo));
        var viewContext = new ViewContext();
        var modelExpression = nameof(Model.Foo);

        var modelHelper = new Mock<IModelHelper>();
        modelHelper.Setup(mock => mock.GetModelValue(viewContext, modelExplorer, modelExpression)).Returns(modelValue);

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
                    attributes: null,
                    content: new HtmlString("The label"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new CharacterCountTagHelper(modelHelper: modelHelper.Object)
        {
            AspFor = new ModelExpression(modelExpression, modelExplorer),
            Name = "my-name",
            MaxWords = 10,
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();
        var textarea = element.GetElementsByTagName("textarea")[0];
        Assert.Equal("Foo value", textarea.InnerHtml);
    }

    [Fact]
    public async Task ProcessAsync_WithValue_RendersTextAreaWithValue()
    {
        // Arrange
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
                    attributes: null,
                    content: new HtmlString("The label"));

                characterCountContext.SetValue(new HtmlString("Value"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new CharacterCountTagHelper()
        {
            Name = "my-name",
            MaxWords = 10,
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();
        var textarea = element.GetElementsByTagName("textarea")[0];
        Assert.Equal("Value", textarea.InnerHtml);
    }

    [Fact]
    public async Task ProcessAsync_WithValueAndAspFor_RendersTextAreaWithValue()
    {
        // Arrange
        var modelValue = "Foo value";
        var model = new Model()
        {
            Foo = modelValue
        };

        var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), model)
            .GetExplorerForProperty(nameof(Model.Foo));
        var viewContext = new ViewContext();
        var modelExpression = nameof(Model.Foo);

        var modelHelper = new Mock<IModelHelper>();
        modelHelper.Setup(mock => mock.GetModelValue(viewContext, modelExplorer, modelExpression)).Returns(modelValue);

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
                    attributes: null,
                    content: new HtmlString("The label"));

                characterCountContext.SetValue(new HtmlString("Value"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new CharacterCountTagHelper(modelHelper: modelHelper.Object)
        {
            AspFor = new ModelExpression(modelExpression, modelExplorer),
            Name = "my-name",
            MaxWords = 10,
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();
        var textarea = element.GetElementsByTagName("textarea")[0];
        Assert.Equal("Value", textarea.InnerHtml);
    }

    [Theory]
    [InlineData(-1)]
    public void SetThreshold_InvalidValue_ThrowsArgumentException(decimal threshold)
    {
        // Arrange

        // Act
        var ex = Record.Exception(() => new CharacterCountTagHelper()
        {
            Id = "my-id",
            Name = "my-name",
            Threshold = threshold
        });

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(ex);
        Assert.StartsWith("Threshold cannot be less than 0.", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void SetMaxLength_InvalidValue_ThrowsArgumentException(int maxLength)
    {
        // Arrange

        // Act
        var ex = Record.Exception(() => new CharacterCountTagHelper()
        {
            Id = "my-id",
            MaxLength = maxLength,
            Name = "my-name"
        });

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(ex);
        Assert.StartsWith("MaxLength must be greater than 0.", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void SetMaxWords_InvalidValue_ThrowsArgumentException(int maxWords)
    {
        // Arrange

        // Act
        var ex = Record.Exception(() => new CharacterCountTagHelper()
        {
            Id = "my-id",
            MaxWords = maxWords,
            Name = "my-name"
        });

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(ex);
        Assert.StartsWith("MaxWords must be greater than 0.", ex.Message);
    }

    private class Model
    {
        public string? Foo { get; set; }
    }
}
