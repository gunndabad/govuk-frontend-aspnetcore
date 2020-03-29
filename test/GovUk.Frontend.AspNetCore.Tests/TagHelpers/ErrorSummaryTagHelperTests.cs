using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class ErrorSummaryTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-summary",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var errorSummaryContext = (ErrorSummaryContext)context.Items[ErrorSummaryContext.ContextName];

                    errorSummaryContext.TrySetTitle(attributes: null, new HtmlString("Title"));
                    errorSummaryContext.TrySetDescription(attributes: null, new HtmlString("Description"));

                    errorSummaryContext.AddItem(new ErrorSummaryItem()
                    {
                        Content = new HtmlString("First message")
                    });

                    errorSummaryContext.AddItem(new ErrorSummaryItem()
                    {
                        Content = new HtmlString("Second message")
                    });

                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryTagHelper(new DefaultGovUkHtmlGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(
                "<div aria-labelledby=\"error-summary-title\" class=\"govuk-error-summary\" data-module=\"govuk-error-summary\" role=\"alert\" tabindex=\"-1\">" +
                "<h2 class=\"govuk-error-summary__title\" id=\"error-summary-title\">Title</h2>" +
                "<div class=\"govuk-error-summary__body\">" +
                "<p>Description</p>" +
                "<ul class=\"govuk-error-summary__list govuk-list\">" +
                "<li>First message</li>" +
                "<li>Second message</li>" +
                "</ul>" +
                "</div>" +
                "</div>",
                output.AsString());
        }

        [Fact]
        public async Task ProcessAsync_NoTitleThrowsInvalidOperationException()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-error-summary",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryTagHelper(new DefaultGovUkHtmlGenerator());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Missing <govuk-error-summary-title> element.", ex.Message);
        }
    }

    public class ErrorSummaryTitleTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsTitleToContext()
        {
            // Arrange
            var errorSummaryContext = new ErrorSummaryContext();

            var context = new TagHelperContext(
                tagName: "govuk-error-summary-title",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { ErrorSummaryContext.ContextName, errorSummaryContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary-title",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Some title");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryTitleTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Some title", errorSummaryContext.Title?.content.AsString());
        }

        [Fact]
        public async Task ProcessAsync_AlreadyHaveTitleThrowsInvalidOperationException()
        {
            // Arrange
            var errorSummaryContext = new ErrorSummaryContext();
            errorSummaryContext.TrySetTitle(attributes: null, new HtmlString("Existing title"));

            var context = new TagHelperContext(
                tagName: "govuk-error-summary-title",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { ErrorSummaryContext.ContextName, errorSummaryContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary-title",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Some title");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryTitleTagHelper();

            // Act
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot render <govuk-error-summary-title> here.", ex.Message);
        }
    }

    public class ErrorSummaryDescriptionTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsDescriptionToContext()
        {
            // Arrange
            var errorSummaryContext = new ErrorSummaryContext();

            var context = new TagHelperContext(
                tagName: "govuk-error-summary-description",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { ErrorSummaryContext.ContextName, errorSummaryContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary-description",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Some description");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryDescriptionTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Some description", errorSummaryContext.Description?.content.AsString());
        }

        [Fact]
        public async Task ProcessAsync_AlreadyHaveDescriptionThrowsInvalidOperationException()
        {
            // Arrange
            var errorSummaryContext = new ErrorSummaryContext();
            errorSummaryContext.TrySetDescription(attributes: null, new HtmlString("Existing description"));

            var context = new TagHelperContext(
                tagName: "govuk-error-summary-description",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { ErrorSummaryContext.ContextName, errorSummaryContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary-description",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Some description");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryDescriptionTagHelper();

            // Act
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Cannot render <govuk-error-summary-description> here.", ex.Message);
        }
    }

    public class ErrorSummaryItemTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_AddsItemToContext()
        {
            // Arrange
            var errorSummaryContext = new ErrorSummaryContext();

            var context = new TagHelperContext(
                tagName: "govuk-error-summary-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { ErrorSummaryContext.ContextName, errorSummaryContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("An error message");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryItemTagHelper(new DefaultGovUkHtmlGenerator());

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(1, errorSummaryContext.Items.Count);

            var firstItem = errorSummaryContext.Items.First();
            Assert.Equal("An error message", firstItem.Content.AsString());
        }

        [Fact]
        public async Task ProcessAsync_NoContentAndNoAspForThrowsInvalidOperationException()
        {
            // Arrange
            var errorSummaryContext = new ErrorSummaryContext();

            var context = new TagHelperContext(
                tagName: "govuk-error-summary-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { ErrorSummaryContext.ContextName, errorSummaryContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("An error message");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            output.TagMode = TagMode.SelfClosing;

            var tagHelper = new ErrorSummaryItemTagHelper(new DefaultGovUkHtmlGenerator());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
            Assert.Equal("Content is required when the 'asp-for' attribute is not specified.", ex.Message);
        }

        [Fact]
        public async Task ProcessAsync_LinkAttributesSpecifiedGeneratesExpectedContent()
        {
            // Arrange
            var errorSummaryContext = new ErrorSummaryContext();

            var context = new TagHelperContext(
                tagName: "govuk-error-summary-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { ErrorSummaryContext.ContextName, errorSummaryContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("An error message");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new ErrorSummaryItemTagHelper(new DefaultGovUkHtmlGenerator())
            {
                For = "field"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(1, errorSummaryContext.Items.Count);

            var firstItem = errorSummaryContext.Items.First();
            Assert.Equal("<a href=\"#field\">An error message</a>", firstItem.Content.AsString());
        }

        [Fact]
        public async Task ProcessAsync_AspForSpecifiedGeneratesExpectedContent()
        {
            // Arrange
            var errorSummaryContext = new ErrorSummaryContext();

            var context = new TagHelperContext(
                tagName: "govuk-error-summary-item",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { ErrorSummaryContext.ContextName, errorSummaryContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-error-summary-item",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            output.TagMode = TagMode.SelfClosing;

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
                .Setup(mock => mock.GetValidationMessage(
                    /*viewContext: */It.IsAny<ViewContext>(),
                    /*modelExplorer: */It.IsAny<ModelExplorer>(),
                    /*expression: */It.IsAny<string>()))
                .Returns("Generated error");

            var modelExplorer = new EmptyModelMetadataProvider().GetModelExplorerForType(typeof(Model), "Foo");

            var tagHelper = new ErrorSummaryItemTagHelper(htmlGenerator.Object)
            {
                AspFor = new ModelExpression("Foo", modelExplorer),
                ViewContext = new ViewContext()
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(1, errorSummaryContext.Items.Count);

            var firstItem = errorSummaryContext.Items.First();
            Assert.Equal("<a href=\"#Foo\">Generated error</a>", firstItem.Content.AsString());
        }
    }
}
