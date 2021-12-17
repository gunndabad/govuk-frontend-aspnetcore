using System;
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

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class FormGroupTagHelperBaseTests
    {
        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "new", "new")]
        [InlineData("initial", null, "initial")]
        [InlineData("initial", "", "initial")]
        [InlineData("initial", "new", "initial new")]
        public void AppendToDescribedBy(string initialValue, string newValue, string expectedResult)
        {
            // Arrange

            // Act
            var result = FormGroupTagHelperBase.AppendToDescribedBy(initialValue, newValue);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GenerateErrorMessage_ErrorMessageSetOnContext_ReturnsContent()
        {
            // Arrange
            var context = new TestFormGroupContext();
            context.SetErrorMessage(visuallyHiddenText: "vht", attributes: null, content: new HtmlString("Error message"));

            var modelHelper = new Mock<IModelHelper>();

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                Id = "test"
            };

            // Act
            var errorMessage = tagHelper.GenerateErrorMessage(context);

            // Assert
            Assert.NotNull(errorMessage);
        }

        [Fact]
        public void GenerateErrorMessage_AspForModelStateHasErrors_ReturnsContent()
        {
            // Arrange
            var context = new TestFormGroupContext();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
                .GetExplorerForProperty(nameof(Model.SimpleProperty));
            var viewContext = new ViewContext();
            var modelExpression = nameof(Model.SimpleProperty);

            var modelHelper = new Mock<IModelHelper>();
            modelHelper.Setup(mock => mock.GetValidationMessage(viewContext, modelExplorer, modelExpression)).Returns("ModelState error");

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                AspFor = new ModelExpression(modelExpression, modelExplorer),
                Id = "test",
                ViewContext = viewContext
            };

            // Act
            var errorMessage = tagHelper.GenerateErrorMessage(context);

            // Assert
            Assert.NotNull(errorMessage);
        }

        [Fact]
        public void GenerateErrorMessage_ErrorMessageOnContextAndModelStateUsesContextError_ReturnsContent()
        {
            // Arrange
            var context = new TestFormGroupContext();
            context.SetErrorMessage(visuallyHiddenText: "vht", attributes: null, content: new HtmlString("Context error"));

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
                .GetExplorerForProperty(nameof(Model.SimpleProperty));
            var viewContext = new ViewContext();
            var modelExpression = nameof(Model.SimpleProperty);

            var modelHelper = new Mock<IModelHelper>();
            modelHelper.Setup(mock => mock.GetValidationMessage(viewContext, modelExplorer, modelExpression)).Returns("ModelState error");

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                AspFor = new ModelExpression(modelExpression, modelExplorer),
                Id = "test",
                ViewContext = viewContext
            };

            // Act
            var errorMessage = tagHelper.GenerateErrorMessage(context);

            // Assert
            Assert.NotNull(errorMessage);
            Assert.Contains("Context error", errorMessage.RenderToString());
        }

        [Fact]
        public void GenerateErrorMessage_AspForModelStateHasErrorsButIgnoreModelStateErrorsSet_ReturnsNull()
        {
            // Arrange
            var context = new TestFormGroupContext();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
                .GetExplorerForProperty(nameof(Model.SimpleProperty));
            var viewContext = new ViewContext();
            var modelExpression = nameof(Model.SimpleProperty);

            var modelHelper = new Mock<IModelHelper>();
            modelHelper.Setup(mock => mock.GetValidationMessage(viewContext, modelExplorer, modelExpression)).Returns("ModelState error");

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                AspFor = new ModelExpression(modelExpression, modelExplorer),
                Id = "test",
                IgnoreModelStateErrors = true,
                ViewContext = viewContext
            };

            // Act
            var errorMessage = tagHelper.GenerateErrorMessage(context);

            // Assert
            Assert.Null(errorMessage);
        }

        [Fact]
        public void GenerateErrorMessage_NoErrorMessageOnContextOrModelStateErrors_ReturnsNull()
        {
            // Arrange
            var context = new TestFormGroupContext();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
                .GetExplorerForProperty(nameof(Model.SimpleProperty));
            var viewContext = new ViewContext();
            var modelExpression = "Foo";

            var modelHelper = new Mock<IModelHelper>();
            modelHelper.Setup(mock => mock.GetValidationMessage(viewContext, modelExplorer, modelExpression)).Returns((string)null);

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                AspFor = new ModelExpression(modelExpression, modelExplorer),
                Id = "test",
                ViewContext = viewContext
            };

            // Act
            var errorMessage = tagHelper.GenerateErrorMessage(context);

            // Assert
            Assert.Null(errorMessage);
        }

        [Fact]
        public void GenerateErrorMessage_NoErrorMessageOnContextOrAspFor_ReturnsNull()
        {
            // Arrange
            var context = new TestFormGroupContext();

            var modelHelper = new Mock<IModelHelper>();

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                Id = "test"
            };

            // Act
            var errorMessage = tagHelper.GenerateErrorMessage(context);

            // Assert
            Assert.Null(errorMessage);
        }

        [Fact]
        public void GenerateErrorMessage_NonEmptyErrorMessage_AddsErrorMessageIdToDescribedBy()
        {
            // Arrange
            var context = new TestFormGroupContext();
            context.SetErrorMessage(visuallyHiddenText: null, attributes: null, content: new HtmlString("Context error"));

            var modelHelper = new Mock<IModelHelper>();

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                Id = "test"
            };

            // Act
            var errorMessage = tagHelper.GenerateErrorMessage(context);

            // Assert
            var element = errorMessage.RenderToElement();
            Assert.Equal("test-error", element.GetAttribute("id"));
            Assert.Contains("test-error", tagHelper.DescribedBy?.Split(' ') ?? Array.Empty<string>());
        }

        [Fact]
        public void GenerateErrorMessage_EmptyErrorMessage_DoesNotAddErrorMessageIdToDescribedBy()
        {
            // Arrange
            var context = new TestFormGroupContext();

            var modelHelper = new Mock<IModelHelper>();

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                Id = "test"
            };

            // Act
            tagHelper.GenerateErrorMessage(context);

            // Assert
            Assert.DoesNotContain("test-error", tagHelper.DescribedBy?.Split(' ') ?? Array.Empty<string>());
        }

        [Fact]
        public void GenerateHint_HintOnContext_ReturnsContent()
        {
            // Arrange
            var context = new TestFormGroupContext();
            context.SetHint(attributes: null, content: new HtmlString("Hint"));

            var modelHelper = new Mock<IModelHelper>();

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                Id = "test"
            };

            // Act
            var hint = tagHelper.GenerateHint(context);

            // Assert
            Assert.NotNull(hint);
        }

        [Fact]
        public void GenerateHint_NoHintOnContext_ReturnsNull()
        {
            // Arrange
            var context = new TestFormGroupContext();

            var modelHelper = new Mock<IModelHelper>();

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                Id = "test"
            };

            // Act
            var hint = tagHelper.GenerateHint(context);

            // Assert
            Assert.Null(hint);
        }

        [Fact]
        public void GenerateHint_NonEmptyHint_AddsHintIdToDescribedBy()
        {
            // Arrange
            var context = new TestFormGroupContext();
            context.SetHint(attributes: null, content: new HtmlString("Hint"));

            var modelHelper = new Mock<IModelHelper>();

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                Id = "test"
            };

            // Act
            var hint = tagHelper.GenerateHint(context);

            // Assert
            var element = hint.RenderToElement();
            Assert.Equal("test-hint", element.GetAttribute("id"));
            Assert.Contains("test-hint", tagHelper.DescribedBy?.Split(' ') ?? Array.Empty<string>());
        }

        [Fact]
        public void GenerateHint_EmptyHint_DoesNotAddHintIdToDescribedBy()
        {
            // Arrange
            var context = new TestFormGroupContext();

            var modelHelper = new Mock<IModelHelper>();

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                Id = "test"
            };

            // Act
            tagHelper.GenerateHint(context);

            // Assert
            Assert.DoesNotContain("test-hint", tagHelper.DescribedBy?.Split(' ') ?? Array.Empty<string>());
        }

        [Fact]
        public void GenerateLabel_NoLabelContentOnContextOrAspFor_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TestFormGroupContext();

            var modelHelper = new Mock<IModelHelper>();

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                Id = "test"
            };

            // Act
            var ex = Record.Exception(() => tagHelper.GenerateLabel(context));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Label content must be specified when the 'asp-for' attribute is not specified.", ex.Message);
        }

        [Fact]
        public void GenerateLabel_LabelContentOnContext_ReturnsContentFromContext()
        {
            // Arrange
            var context = new TestFormGroupContext();
            context.SetLabel(isPageHeading: false, attributes: null, content: new HtmlString("Context label"));

            var modelHelper = new Mock<IModelHelper>();

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                Id = "test"
            };

            // Act
            var label = tagHelper.GenerateLabel(context);

            // Assert
            Assert.NotNull(label);
            Assert.Contains("Context label", label.RenderToString());
        }

        [Fact]
        public void GenerateLabel_NoLabelContentOnContext_ReturnsContentFromAspFor()
        {
            // Arrange
            var context = new TestFormGroupContext();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
                .GetExplorerForProperty(nameof(Model.SimpleProperty));
            var viewContext = new ViewContext();
            var modelExpression = nameof(Model.SimpleProperty);

            var modelHelper = new Mock<IModelHelper>();
            modelHelper.Setup(mock => mock.GetDisplayName(viewContext, modelExplorer, modelExpression)).Returns("ModelMetadata label");

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                AspFor = new ModelExpression(modelExpression, modelExplorer),
                Id = "test",
                ViewContext = viewContext
            };

            // Act
            var label = tagHelper.GenerateLabel(context);

            // Assert
            Assert.NotNull(label);
            Assert.Contains("ModelMetadata label", label.RenderToString());
        }

        [Fact]
        public void GenerateLabel_LabelContentOnContextAndAspFor_ReturnsContentFromContext()
        {
            // Arrange
            var context = new TestFormGroupContext();
            context.SetLabel(isPageHeading: false, attributes: null, content: new HtmlString("Context label"));

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), new Model())
                .GetExplorerForProperty(nameof(Model.SimpleProperty));
            var viewContext = new ViewContext();
            var modelExpression = nameof(Model.SimpleProperty);

            var modelHelper = new Mock<IModelHelper>();
            modelHelper.Setup(mock => mock.GetDisplayName(viewContext, modelExplorer, modelExpression)).Returns("ModelMetadata label");

            var tagHelper = new TestFormGroupTagHelper(new ComponentGenerator(), modelHelper.Object)
            {
                AspFor = new ModelExpression(modelExpression, modelExplorer),
                Id = "test",
                ViewContext = viewContext
            };

            // Act
            var label = tagHelper.GenerateLabel(context);

            // Assert
            Assert.NotNull(label);
            Assert.Contains("Context label", label.RenderToString());
        }

        private class Model
        {
            public string SimpleProperty { get; set; }
        }

        private class TestFormGroupTagHelper : FormGroupTagHelperBase
        {
            public TestFormGroupTagHelper(IGovUkHtmlGenerator htmlGenerator, IModelHelper modelHelper) :
                base(htmlGenerator, modelHelper)
            {
            }

            public string Id { get; set; }

            private protected override FormGroupContext CreateFormGroupContext() => new TestFormGroupContext();

            private protected override IHtmlContent GenerateFormGroupContent(
                TagHelperContext context,
                FormGroupContext formGroupContext,
                out bool haveError)
            {
                var contentBuilder = new HtmlContentBuilder();

                var label = GenerateLabel(formGroupContext);
                contentBuilder.AppendHtml(label);

                var hint = GenerateHint(formGroupContext);
                if (hint != null)
                {
                    contentBuilder.AppendHtml(hint);
                }

                var errorMessage = GenerateErrorMessage(formGroupContext);
                if (errorMessage != null)
                {
                    contentBuilder.AppendHtml(errorMessage);
                }

                haveError = errorMessage != null;

                contentBuilder.Append("Test content");

                return contentBuilder;
            }

            private protected override string ResolveId() => Id;
        }

        private class TestFormGroupContext : FormGroupContext
        {
            protected override string ErrorMessageTagName => "test-error-message";

            protected override string HintTagName => "test-hint";

            protected override string LabelTagName => "test-label";

            protected override string RootTagName => "test";
        }
    }
}
