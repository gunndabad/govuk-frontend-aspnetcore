using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
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

            var htmlGenerator = new Mock<IHtmlGenerator>();
            htmlGenerator
                .Setup(
                    mock => mock.GenerateLabel(
                        /*viewContext: */It.IsAny<ViewContext>(),
                        /*modelExplorer: */It.IsAny<ModelExplorer>(),
                        /*expression: */It.IsAny<string>(),
                        /*labelText: */It.IsAny<string>(),
                        /*htmlAttributes: */It.IsAny<object>()))
                .Returns(new TagBuilder("label"));

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(htmlGenerator.Object))
            {
                For = "some-input-id"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("label", output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
            Assert.Equal("govuk-label", output.Attributes["class"].Value);
            Assert.Equal("some-input-id", output.Attributes["for"].Value);
            Assert.Equal("Label content", output.Content.GetContent());
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

            var htmlGenerator = new Mock<IHtmlGenerator>();
            htmlGenerator
                .Setup(
                    mock => mock.GenerateLabel(
                        /*viewContext: */It.IsAny<ViewContext>(),
                        /*modelExplorer: */It.IsAny<ModelExplorer>(),
                        /*expression: */It.IsAny<string>(),
                        /*labelText: */It.IsAny<string>(),
                        /*htmlAttributes: */It.IsAny<object>()))
                .Returns(() =>
                {
                    var tagBuilder = new TagBuilder("label");
                    tagBuilder.Attributes.Add("for", "Foo");
                    tagBuilder.InnerHtml.Append("Generated label");
                    return tagBuilder;
                });

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(htmlGenerator.Object))
            {
                AspFor = new ModelExpression("Foo", modelExplorer)
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("label", output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
            Assert.Equal("govuk-label", output.Attributes["class"].Value);
            Assert.Equal("Foo", output.Attributes["for"].Value);
            Assert.Equal("Generated label", output.Content.GetContent());
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

            var htmlGenerator = new Mock<IHtmlGenerator>();
            htmlGenerator
                .Setup(
                    mock => mock.GenerateLabel(
                        /*viewContext: */It.IsAny<ViewContext>(),
                        /*modelExplorer: */It.IsAny<ModelExplorer>(),
                        /*expression: */It.IsAny<string>(),
                        /*labelText: */It.IsAny<string>(),
                        /*htmlAttributes: */It.IsAny<object>()))
                .Returns(() =>
                {
                    var tagBuilder = new TagBuilder("label");
                    tagBuilder.Attributes.Add("for", "Foo");
                    tagBuilder.InnerHtml.Append("Generated label");
                    return tagBuilder;
                });

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(htmlGenerator.Object))
            {
                AspFor = new ModelExpression("Foo", modelExplorer)
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("label", output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
            Assert.Equal("govuk-label", output.Attributes["class"].Value);
            Assert.Equal("Foo", output.Attributes["for"].Value);
            Assert.Equal("Specific content", output.Content.GetContent());
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

            var htmlGenerator = new Mock<IHtmlGenerator>();
            htmlGenerator
                .Setup(
                    mock => mock.GenerateLabel(
                        /*viewContext: */It.IsAny<ViewContext>(),
                        /*modelExplorer: */It.IsAny<ModelExplorer>(),
                        /*expression: */It.IsAny<string>(),
                        /*labelText: */It.IsAny<string>(),
                        /*htmlAttributes: */It.IsAny<object>()))
                .Returns(new TagBuilder("label"));

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(htmlGenerator.Object))
            {
                For = "some-input-id",
                IsPageHeading = true
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("h1", output.TagName);
            Assert.Equal("govuk-label-wrapper", output.Attributes["class"].Value);
            Assert.Equal("<label class=\"govuk-label\" for=\"some-input-id\">Label content</label>", output.Content.GetContent());
        }

        [Fact]
        public async Task ProcessAsync_BothAspForAndForSpecifiedThrowsInvalidOperationException()
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

            var htmlGenerator = new Mock<IHtmlGenerator>();

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(htmlGenerator.Object))
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                For = "some-input-id"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot specify both the 'asp-for' and 'for' attributes.", ex.Message);
        }

        public class Model
        {
            public string Foo { get; set; }
        }
    }
}
