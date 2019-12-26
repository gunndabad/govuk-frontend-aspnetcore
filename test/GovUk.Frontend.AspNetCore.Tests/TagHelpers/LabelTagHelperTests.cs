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

            var htmlHelper = new Mock<IHtmlHelper>();
            htmlHelper
                .Setup(mock => mock.Id(It.IsAny<string>()))
                .Returns("another-id");

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(), htmlHelper.Object)
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

            var htmlHelper = new Mock<IHtmlHelper>();

            htmlHelper
                .Setup(mock => mock.Id(It.IsAny<string>()))
                .Returns("Foo");

            htmlHelper
                .Setup(mock => mock.DisplayName(It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(), htmlHelper.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer)
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

            var htmlHelper = new Mock<IHtmlHelper>();

            htmlHelper
                .Setup(mock => mock.Id(It.IsAny<string>()))
                .Returns("Foo");

            htmlHelper
                .Setup(mock => mock.DisplayName(It.IsAny<string>()))
                .Returns("Generated label");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(), htmlHelper.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer)
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

            var htmlHelper = new Mock<IHtmlHelper>();

            var tagHelper = new LabelTagHelper(new DefaultGovUkHtmlGenerator(), htmlHelper.Object)
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
