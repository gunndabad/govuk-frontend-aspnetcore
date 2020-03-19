using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class FormGroupTagHelperBaseTests
    {
        [Fact]
        public async Task ProcessAsync_NoNameOrAspForThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupTagHelper(Mock.Of<IGovUkHtmlGenerator>())
            {
                Id = "my-element-id"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("At least one of the 'name' and 'asp-for' attributes must be specified.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_NonNullIdUsesSpecifiedId()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                Id = "my-element-id",
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            Assert.Equal("my-element-id", node.ChildNodes.FindFirst("dummy").Attributes["id"].Value);
        }

        [Fact]
        public async Task ProcessAsync_NullIdUsesGeneratedId()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            var node = HtmlNode.CreateNode(html);
            Assert.Equal("Foo", node.ChildNodes.FindFirst("dummy").Attributes["id"].Value);
        }

        [Fact]
        public async Task ProcessAsync_ErrorInModelStateIncludesErrorClass()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            htmlGenerator
                .Setup(mock => mock.GetValidationMessage(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("An error");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Contains("govuk-form-group--error", (string)output.Attributes["class"].Value);
        }

        [Fact]
        public async Task ProcessAsync_ErrorSpecifiedIncludesErrorClass()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        content: new HtmlString("Boom!"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Contains("govuk-form-group--error", (string)output.Attributes["class"].Value);
        }

        [Fact]
        public async Task ProcessAsync_NoLabelContentOrAspForThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "element-id",
                Name = "element-name"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Label content must be specified when the 'asp-for' attribute is not specified.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_LabelContentSpecifiedAndExplicitIdOutputsCorrectLabel()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                Id = "element-id",
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains("<label class=\"govuk-label\" for=\"element-id\">The label</label>", html);
        }

        [Fact]
        public async Task ProcessAsync_LabelContentNotSpecifiedAndExplicitIdOutputsCorrectLabel()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                Id = "element-id",
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains("<label class=\"govuk-label\" for=\"element-id\">Generated label</label>", html);
        }

        [Fact]
        public async Task ProcessAsync_LabelContentSpecifiedButNoExplicitIdOutputsCorrectLabel()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains("<label class=\"govuk-label\" for=\"Foo\">The label</label>", html);
        }

        [Fact]
        public async Task ProcessAsync_LabelContentNotSpecifiedAndNoExplicitIdOutputsCorrectLabel()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains("<label class=\"govuk-label\" for=\"Foo\">Generated label</label>", html);
        }

        [Fact]
        public async Task ProcessAsync_LabelIsPageHeaderSpecifiedButNoContentOutputsCorrectLabel()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: true,
                        attributes: null,
                        content: null);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains(
                "<h1 class=\"govuk-label-wrapper\"><label class=\"govuk-label\" for=\"Foo\">Generated label</label></h1>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_LabelIsPageHeaderSpecifiedAndContentOutputsCorrectLabel()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: true,
                        attributes: null,
                        content: new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains(
                "<h1 class=\"govuk-label-wrapper\"><label class=\"govuk-label\" for=\"Foo\">The label</label></h1>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_NoModelStateErrorsAndNoErrorMessageSpecifiedDoesNotOutputErrorMessage()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        content: null);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            htmlGenerator
                .Setup(mock => mock.GetValidationMessage(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns((string)null);

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.DoesNotContain("<span class=\"govuk-error-message\"", html);
        }

        [Fact]
        public async Task ProcessAsync_ModelStateErrorOutputsCorrectErrorMessage()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        content: null);

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            htmlGenerator
                .Setup(mock => mock.GetValidationMessage(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("An error");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains(
                "<span class=\"govuk-error-message\" id=\"Foo-error\"><span class=\"govuk-visually-hidden\">Error</span>An error</span>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_SpecifiedErrorOutputsCorrectErrorMessage()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: "Bang",
                        attributes: null,
                        content: new HtmlString("Boom!"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var htmlGenerator = new Mock<DefaultGovUkHtmlGenerator>()
            {
                CallBase = true
            };

            htmlGenerator
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            htmlGenerator
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            htmlGenerator
                .Setup(mock => mock.GetValidationMessage(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("An error");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new TestFormGroupTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains(
                "<span class=\"govuk-error-message\" id=\"Foo-error\"><span class=\"govuk-visually-hidden\">Bang</span>Boom!</span>",
                html);
        }

        [Fact]
        public async Task ProcessAsync_HintSpecifiedOutputsExpectedContent()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        new HtmlString("The label"));
                    formGroupContext.TrySetHint(attributes: null, content: new HtmlString("The hint"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "element-id",
                Name = "element-name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains("<span class=\"govuk-hint\" id=\"element-id-hint\">The hint</span>", html);
        }

        [Fact]
        public async Task ProcessAsync_AddsElementToOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "element-id",
                Name = "element-name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains("<dummy have-error=\"False\" id=\"element-id\"></dummy>", html);
        }

        [Fact]
        public async Task ProcessAsync_DescribedBySpecifiedPassedToElement()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        new HtmlString("The label"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupTagHelper(new DefaultGovUkHtmlGenerator())
            {
                DescribedBy = "other-thing",
                Id = "element-id",
                Name = "element-name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains("<dummy described-by=\"other-thing\" have-error=\"False\" id=\"element-id\"></dummy>", html);
        }

        [Fact]
        public async Task ProcessAsync_WithErrorIncludesErrorIdInDescribedBy()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        new HtmlString("The label"));
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        new HtmlString("An error"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "element-id",
                Name = "element-name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains("<dummy described-by=\"element-id-error\" have-error=\"True\" id=\"element-id\"></dummy>", html);
        }

        [Fact]
        public async Task ProcessAsync_WithHintIncludesHintIdInDescribedBy()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        new HtmlString("The label"));
                    formGroupContext.TrySetHint(attributes: null, content: new HtmlString("Hint"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "element-id",
                Name = "element-name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains("<dummy described-by=\"element-id-hint\" have-error=\"False\" id=\"element-id\"></dummy>", html);
        }

        [Fact]
        public async Task ProcessAsync_WithHintErrorMessageAndSpecifiedDescribedByIncludesAllInDescribedBy()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var formGroupContext = (FormGroupBuilder)context.Items[FormGroupBuilder.ContextName];
                    formGroupContext.TrySetLabel(
                        isPageHeading: false,
                        attributes: null,
                        new HtmlString("The label"));
                    formGroupContext.TrySetHint(attributes: null, content: new HtmlString("Hint"));
                    formGroupContext.TrySetErrorMessage(
                        visuallyHiddenText: null,
                        attributes: null,
                        content: new HtmlString("An error"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupTagHelper(new DefaultGovUkHtmlGenerator())
            {
                Id = "element-id",
                DescribedBy = "other-thing",
                Name = "element-name"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Contains("<dummy described-by=\"other-thing element-id-hint element-id-error\" have-error=\"True\" id=\"element-id\"></dummy>", html);
        }
    }

    public class FormGroupLabelTagHelperBaseTests
    {
        [Fact]
        public async Task ProcessAsync_AddsContentToContext()
        {
            // Arrange
            var formGroupContext = new FormGroupBuilder();

            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup-label",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { FormGroupBuilder.ContextName, formGroupContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup-label",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The label content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupLabelTagHelper()
            {
                IsPageHeading = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(formGroupContext.Label.Value.isPageHeading);
            Assert.Equal("The label content", formGroupContext.Label.Value.content.AsString());
            Assert.Null(output.TagName);
        }
    }

    public class FormGroupHintTagHelperBaseTests
    {
        [Fact]
        public async Task ProcessAsync_AddsContentToContext()
        {
            // Arrange
            var formGroupContext = new FormGroupBuilder();

            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup-hint",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { FormGroupBuilder.ContextName, formGroupContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup-hint",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The hint content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupHintTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("The hint content", formGroupContext.Hint?.content.AsString());
            Assert.Null(output.TagName);
        }
    }

    public class FormGroupErrorMessageTagHelperBaseTests
    {
        [Fact]
        public async Task ProcessAsync_AddsContentToContext()
        {
            // Arrange
            var formGroupContext = new FormGroupBuilder();

            var context = new TagHelperContext(
                tagName: "govuk-test-formgroup-error-message",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { FormGroupBuilder.ContextName, formGroupContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-test-formgroup-error-message",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The error content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new TestFormGroupErrorMessageTagHelper()
            {
                VisuallyHiddenText = "vht"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.NotNull(formGroupContext.ErrorMessage);
            Assert.Equal("vht", formGroupContext.ErrorMessage.Value.visuallyHiddenText);
            Assert.Equal("The error content", formGroupContext.ErrorMessage.Value.content.AsString());
            Assert.Null(output.TagName);
        }
    }

    public class FormGroupContextTests
    {
        [Fact]
        public void InitialStageIsNone()
        {
            // Arrange
            var ctx = new FormGroupBuilder();

            // Assert
            Assert.Equal(FormGroupRenderStage.None, ctx.RenderStage);
        }

        [Fact]
        public void TrySetLabel_StageAtNoneReturnsTrue()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            var content = new HtmlString("New label");

            // Act
            var result = ctx.TrySetLabel(isPageHeading: true, attributes: null, content);

            // Assert
            Assert.True(result);
            Assert.Equal(FormGroupRenderStage.Label, ctx.RenderStage);
            Assert.NotNull(ctx.Label);
            Assert.True(ctx.Label.Value.isPageHeading);
            Assert.Same(content, ctx.Label.Value.content);
        }

        [Fact]
        public void TrySetLabel_StageAtLabelReturnsFalse()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            Assert.True(ctx.TrySetLabel(isPageHeading: false, attributes: null, new HtmlString("Existing label")));
            var oldStage = ctx.RenderStage;

            // Act
            var result = ctx.TrySetLabel(isPageHeading: true, attributes: null, content: new HtmlString("New label"));

            // Assert
            Assert.False(result);
            Assert.Equal(oldStage, ctx.RenderStage);
        }

        [Fact]
        public void TrySetLabel_StageAtHintReturnsFalse()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            Assert.True(ctx.TrySetHint(attributes: null, content: new HtmlString("Existing hint")));
            var oldStage = ctx.RenderStage;

            // Act
            var result = ctx.TrySetLabel(isPageHeading: true, attributes: null, content: new HtmlString("New label"));

            // Assert
            Assert.False(result);
            Assert.Equal(oldStage, ctx.RenderStage);
        }

        [Fact]
        public void TrySetLabel_StageAtErrorMessageReturnsFalse()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            Assert.True(ctx.TrySetErrorMessage(null, null, new HtmlString("Error message")));
            var oldStage = ctx.RenderStage;

            // Act
            var result = ctx.TrySetLabel(isPageHeading: true, attributes: null, content: new HtmlString("New label"));

            // Assert
            Assert.False(result);
            Assert.Equal(oldStage, ctx.RenderStage);
        }

        [Fact]
        public void TrySetHint_StageAtNoneReturnsTrue()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            var content = new HtmlString("Hint");

            // Act
            var result = ctx.TrySetHint(attributes: null, content: content);

            // Assert
            Assert.True(result);
            Assert.Equal(FormGroupRenderStage.Hint, ctx.RenderStage);
            Assert.NotNull(ctx.Hint);
            Assert.Equal("Hint", ctx.Hint?.content.AsString());
        }

        [Fact]
        public void TrySetHint_StageAtLabelReturnsTrue()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            Assert.True(ctx.TrySetLabel(isPageHeading: false, attributes: null, content: new HtmlString("Label")));
            var content = new HtmlString("Hint");

            // Act
            var result = ctx.TrySetHint(attributes: null, content);

            // Assert
            Assert.True(result);
            Assert.Equal(FormGroupRenderStage.Hint, ctx.RenderStage);
            Assert.NotNull(ctx.Hint);
            Assert.Equal("Hint", ctx.Hint?.content.AsString());
        }

        [Fact]
        public void TrySetHint_StageAtHintReturnsFalse()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            Assert.True(ctx.TrySetHint(attributes: null, new HtmlString("Existing hint")));
            var oldStage = ctx.RenderStage;

            // Act
            var result = ctx.TrySetHint(attributes: null, new HtmlString("Hint"));

            // Assert
            Assert.False(result);
            Assert.Equal(oldStage, ctx.RenderStage);
        }

        [Fact]
        public void TrySetHint_StageAtErrorMessageReturnsFalse()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            Assert.True(ctx.TrySetErrorMessage(null, null, new HtmlString("Existing hint")));
            var oldStage = ctx.RenderStage;

            // Act
            var result = ctx.TrySetHint(attributes: null, new HtmlString("Hint"));

            // Assert
            Assert.False(result);
            Assert.Equal(oldStage, ctx.RenderStage);
        }

        [Fact]
        public void TrySetErrorMessage_StageAtNoneReturnsTrue()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            var content = new HtmlString("Hint");
            var visuallyHiddenText = "VHT";

            // Act
            var result = ctx.TrySetErrorMessage(visuallyHiddenText, attributes: null, content);

            // Assert
            Assert.True(result);
            Assert.Equal(FormGroupRenderStage.ErrorMessage, ctx.RenderStage);
            Assert.NotNull(ctx.ErrorMessage);
            Assert.Same(content, ctx.ErrorMessage.Value.content);
            Assert.Equal(visuallyHiddenText, ctx.ErrorMessage.Value.visuallyHiddenText);
        }

        [Fact]
        public void TrySetErrorMessage_StageAtLabelReturnsTrue()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            Assert.True(ctx.TrySetLabel(isPageHeading: false, attributes: null, content: new HtmlString("Label")));
            var content = new HtmlString("Hint");
            var visuallyHiddenText = "VHT";

            // Act
            var result = ctx.TrySetErrorMessage(visuallyHiddenText, attributes: null, content);

            // Assert
            Assert.True(result);
            Assert.Equal(FormGroupRenderStage.ErrorMessage, ctx.RenderStage);
            Assert.NotNull(ctx.ErrorMessage);
            Assert.Same(content, ctx.ErrorMessage.Value.content);
            Assert.Equal(visuallyHiddenText, ctx.ErrorMessage.Value.visuallyHiddenText);
        }

        [Fact]
        public void TrySetErrorMessage_StageAtHintReturnsTrue()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            Assert.True(ctx.TrySetHint(attributes: null, new HtmlString("Hint")));
            var content = new HtmlString("Hint");
            var visuallyHiddenText = "VHT";

            // Act
            var result = ctx.TrySetErrorMessage(visuallyHiddenText, attributes: null, content);

            // Assert
            Assert.True(result);
            Assert.Equal(FormGroupRenderStage.ErrorMessage, ctx.RenderStage);
            Assert.NotNull(ctx.ErrorMessage);
            Assert.Same(content, ctx.ErrorMessage.Value.content);
            Assert.Equal(visuallyHiddenText, ctx.ErrorMessage.Value.visuallyHiddenText);
        }

        [Fact]
        public void TrySetErrorMessage_StageAtErrorMessageReturnsFalse()
        {
            // Arrange
            var ctx = new FormGroupBuilder();
            Assert.True(ctx.TrySetErrorMessage(null, attributes: null, new HtmlString("Existing error")));
            var oldStage = ctx.RenderStage;

            // Act
            var result = ctx.TrySetErrorMessage(null, attributes: null, new HtmlString("New error"));

            // Assert
            Assert.False(result);
            Assert.Equal(oldStage, ctx.RenderStage);
        }

        [Fact]
        public void AllStagesAssignable()
        {
            // Arrange
            var ctx = new FormGroupBuilder();

            // Act & Assert
            Assert.True(ctx.TrySetLabel(isPageHeading: false, attributes: null, content: new HtmlString("Label")));
            Assert.True(ctx.TrySetHint(attributes: null, new HtmlString("Hint")));
            Assert.True(ctx.TrySetErrorMessage(visuallyHiddenText: null, attributes: null, new HtmlString("The error")));
        }
    }

    [HtmlTargetElement("govuk-test-formgroup")]
    [RestrictChildren("govuk-test-formgroup-label", "govuk-test-formgroup-hint", "govuk-test-formgroup-error-message")]
    public class TestFormGroupTagHelper : FormGroupTagHelperBase
    {
        public TestFormGroupTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        public string Id { get; set; }

        protected override TagBuilder GenerateElement(FormGroupBuilder builder, FormGroupElementContext context)
        {
            var tagBuilder = new TagBuilder("dummy");
            tagBuilder.Attributes.Add("id", ResolvedId);
            tagBuilder.Attributes.Add("have-error", context.HaveError.ToString());

            if (!string.IsNullOrEmpty(DescribedBy))
            {
                tagBuilder.Attributes.Add("described-by", DescribedBy);
            }

            return tagBuilder;
        }

        protected override string GetIdPrefix() => Id;
    }

    [HtmlTargetElement("govuk-test-formgroup-label", ParentTag = "govuk-test-formgroup")]
    public class TestFormGroupLabelTagHelper : FormGroupLabelTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-test-formgroup-hint", ParentTag = "govuk-test-formgroup")]
    public class TestFormGroupHintTagHelper : FormGroupHintTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-test-formgroup-error-message", ParentTag = "govuk-test-formgroup")]
    public class TestFormGroupErrorMessageTagHelper : FormGroupErrorMessageTagHelperBase
    {
    }

    public class Model
    {
        public string Foo { get; set; }
    }
}
