using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class LabelTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_WithContentAndForAttributeGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-label",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-label",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Label content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(), new DefaultModelHelper())
            {
                For = "some-input-id"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal("<label class=\"govuk-label\" for=\"some-input-id\">Label content</label>", html);
        }

        [Fact]
        public async Task ProcessAsync_WithNoContentAndAspForAttributeGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-label",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-label",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            output.TagMode = TagMode.SelfClosing;

            var modelHelperMock = new Mock<IModelHelper>();

            modelHelperMock
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            modelHelperMock
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(), modelHelperMock.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal("<label class=\"govuk-label\" for=\"Foo\">Generated label</label>", html);
        }

        [Fact]
        public async Task ProcessAsync_WithContentAndAspForAttributeGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-label",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-label",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Specific content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var modelHelperMock = new Mock<IModelHelper>();

            modelHelperMock
                .Setup(mock => mock.GetFullHtmlFieldName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Foo");

            modelHelperMock
                .Setup(mock => mock.GetDisplayName(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(), modelHelperMock.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal("<label class=\"govuk-label\" for=\"Foo\">Specific content</label>", html);
        }

        [Fact]
        public async Task ProcessAsync_IsPageHeadingSpecifiedGeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-label",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-label",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Label content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(), new DefaultModelHelper())
            {
                For = "some-input-id",
                IsPageHeading = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var html = output.AsString();
            Assert.Equal(
                "<h1 class=\"govuk-label-wrapper\">" +
                "<label class=\"govuk-label\" for=\"some-input-id\">Label content</label>" +
                "</h1>",
                html);
        }

        public class Model
        {
            public string Foo { get; set; }
        }
    }
}
