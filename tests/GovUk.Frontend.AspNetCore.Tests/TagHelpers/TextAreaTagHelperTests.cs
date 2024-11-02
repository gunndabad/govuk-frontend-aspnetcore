using System.Collections.Generic;
using System.Threading.Tasks;
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

public class TextAreaTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-textarea",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-textarea",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var textAreaContext = context.GetContextItem<TextAreaContext>();

                textAreaContext.SetLabel(isPageHeading: false, attributes: null, content: new HtmlString("The label"));

                textAreaContext.SetHint(attributes: null, content: new HtmlString("The hint"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new TextAreaTagHelper()
        {
            Id = "my-id",
            Name = "my-name",
            LabelClass = "additional-label-class",
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml =
            @"
<div class=""govuk-form-group"">
    <label class=""govuk-label additional-label-class"" for=""my-id"">The label</label>
    <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
    <textarea class=""govuk-textarea govuk-js-textarea"" id=""my-id"" name=""my-name"" rows=""5"" aria-describedby=""my-id-hint""></textarea>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_WithError_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-textarea",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-textarea",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var textAreaContext = context.GetContextItem<TextAreaContext>();

                textAreaContext.SetLabel(isPageHeading: false, attributes: null, content: new HtmlString("The label"));

                textAreaContext.SetHint(attributes: null, content: new HtmlString("The hint"));

                textAreaContext.SetErrorMessage(
                    visuallyHiddenText: null,
                    attributes: null,
                    content: new HtmlString("The error")
                );

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new TextAreaTagHelper() { Id = "my-id", Name = "my-name" };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml =
            @"
<div class=""govuk-form-group govuk-form-group--error"">
    <label class=""govuk-label"" for=""my-id"">The label</label>
    <div class=""govuk-hint"" id=""my-id-hint"">The hint</div>
    <p id=""my-id-error"" class=""govuk-error-message"">
        <span class=""govuk-visually-hidden"">Error:</span>
        The error
    </p>
    <textarea class=""govuk-textarea govuk-js-textarea govuk-textarea--error"" id=""my-id"" name=""my-name"" rows=""5"" aria-describedby=""my-id-hint my-id-error""></textarea>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_NoValueOrAspFor_RendersEmptyTextArea()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-textarea",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-textarea",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var textAreaContext = context.GetContextItem<TextAreaContext>();

                textAreaContext.SetLabel(isPageHeading: false, attributes: null, content: new HtmlString("The label"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new TextAreaTagHelper() { Name = "my-name" };

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
        var model = new Model() { Foo = modelValue };

        var modelExplorer = new EmptyModelMetadataProvider()
            .GetModelExplorerForType(typeof(Model), model)
            .GetExplorerForProperty(nameof(Model.Foo));
        var viewContext = new ViewContext();
        var modelExpression = nameof(Model.Foo);

        var modelHelper = new Mock<IModelHelper>();
        modelHelper.Setup(mock => mock.GetModelValue(viewContext, modelExplorer, modelExpression)).Returns(modelValue);

        var context = new TagHelperContext(
            tagName: "govuk-textarea",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-textarea",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var textAreaContext = context.GetContextItem<TextAreaContext>();

                textAreaContext.SetLabel(isPageHeading: false, attributes: null, content: new HtmlString("The label"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new TextAreaTagHelper(modelHelper: modelHelper.Object)
        {
            AspFor = new ModelExpression(modelExpression, modelExplorer),
            Name = "my-name",
            ViewContext = viewContext,
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
            tagName: "govuk-textarea",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-textarea",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var textAreaContext = context.GetContextItem<TextAreaContext>();

                textAreaContext.SetLabel(isPageHeading: false, attributes: null, content: new HtmlString("The label"));

                textAreaContext.SetValue(new HtmlString("Value"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new TextAreaTagHelper() { Name = "my-name" };

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
        var model = new Model() { Foo = modelValue };

        var modelExplorer = new EmptyModelMetadataProvider()
            .GetModelExplorerForType(typeof(Model), model)
            .GetExplorerForProperty(nameof(Model.Foo));
        var viewContext = new ViewContext();
        var modelExpression = nameof(Model.Foo);

        var modelHelper = new Mock<IModelHelper>();
        modelHelper.Setup(mock => mock.GetModelValue(viewContext, modelExplorer, modelExpression)).Returns(modelValue);

        var context = new TagHelperContext(
            tagName: "govuk-textarea",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-textarea",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var textAreaContext = context.GetContextItem<TextAreaContext>();

                textAreaContext.SetLabel(isPageHeading: false, attributes: null, content: new HtmlString("The label"));

                textAreaContext.SetValue(new HtmlString("Value"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new TextAreaTagHelper(modelHelper: modelHelper.Object)
        {
            AspFor = new ModelExpression(modelExpression, modelExplorer),
            Name = "my-name",
            ViewContext = viewContext,
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var element = output.RenderToElement();
        var textarea = element.GetElementsByTagName("textarea")[0];
        Assert.Equal("Value", textarea.InnerHtml);
    }

    private class Model
    {
        public string? Foo { get; set; }
    }
}
