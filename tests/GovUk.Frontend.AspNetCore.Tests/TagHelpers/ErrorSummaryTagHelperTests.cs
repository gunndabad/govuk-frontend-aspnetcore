using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ErrorSummaryTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var firstErrorHtml = "First error";
        var firstErrorHref = "#FirstError";
        var secondErrorHtml = "Second error";
        var secondErrorHref = "#SecondError";
        var disableAutoFocus = true;

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
                var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

                errorSummaryContext.SetTitle(new AttributeCollection(), "Title");
                errorSummaryContext.SetDescription(new AttributeCollection(), "Description");

                errorSummaryContext.AddItem(
                    new ErrorSummaryContextItem(
                        firstErrorHref,
                        firstErrorHtml,
                        new AttributeCollection(),
                        new AttributeCollection()));

                errorSummaryContext.AddItem(
                    new ErrorSummaryContextItem(
                        secondErrorHref,
                        secondErrorHtml,
                        new AttributeCollection(),
                        new AttributeCollection()));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var viewContext = TestUtils.CreateViewContext();
        var containerErrorContext = viewContext.HttpContext.GetContainerErrorContext();

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        ErrorSummaryOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateErrorSummary(It.IsAny<ErrorSummaryOptions>())).Callback<ErrorSummaryOptions>(o => actualOptions = o);

        var tagHelper = new ErrorSummaryTagHelper(componentGeneratorMock.Object)
        {
            DisableAutoFocus = disableAutoFocus,
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.NotNull(actualOptions.ErrorList);
        Assert.Collection(
            actualOptions.ErrorList,
            error =>
            {
                Assert.Equal(firstErrorHref, error.Href);
                Assert.Equal(firstErrorHtml, error.Html);
            },
            error =>
            {
                Assert.Equal(secondErrorHref, error.Href);
                Assert.Equal(secondErrorHtml, error.Html);
            });
        Assert.Equal(disableAutoFocus, actualOptions.DisableAutoFocus);
        Assert.True(containerErrorContext.ErrorSummaryHasBeenRendered);
    }

    [Fact]
    public async Task ProcessAsync_NoTitleSpecified_UsesDefaultTitle()
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
                var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

                errorSummaryContext.AddItem(
                    new ErrorSummaryContextItem(
                        "#Href",
                        "Content",
                        new AttributeCollection(),
                        new AttributeCollection()));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        ErrorSummaryOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateErrorSummary(It.IsAny<ErrorSummaryOptions>())).Callback<ErrorSummaryOptions>(o => actualOptions = o);

        var tagHelper = new ErrorSummaryTagHelper(componentGeneratorMock.Object)
        {
            ViewContext = TestUtils.CreateViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("There is a problem", actualOptions?.TitleHtml);
    }

    [Fact]
    public async Task ProcessAsync_NoTitleDescriptionOrItems_RendersNothing()
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

        var tagHelper = new ErrorSummaryTagHelper(new DefaultComponentGenerator())
        {
            ViewContext = TestUtils.CreateViewContext()
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var html = output.ToHtmlString();
        Assert.Empty(html);
    }

    [Fact]
    public async Task ProcessAsync_HasExplicitItemsDoesNotGetErrorsFromContainerErrorContext()
    {
        // Arrange
        var containerErrorContextErrorHtml = "First error";
        var containerErrorContextErrorHref = "#FirstError";
        var itemErrorHtml = "Item error";
        var itemErrorHref = "#ItemError";

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
                var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

                errorSummaryContext.SetTitle(new AttributeCollection(), "Title");
                errorSummaryContext.SetDescription(new AttributeCollection(), "Description");

                errorSummaryContext.AddItem(
                    new ErrorSummaryContextItem(
                        itemErrorHref,
                        itemErrorHtml,
                        new AttributeCollection(),
                        new AttributeCollection()));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var viewContext = TestUtils.CreateViewContext();
        var containerErrorContext = viewContext.HttpContext.GetContainerErrorContext();
        containerErrorContext.AddError(containerErrorContextErrorHtml, containerErrorContextErrorHref);

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        ErrorSummaryOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateErrorSummary(It.IsAny<ErrorSummaryOptions>())).Callback<ErrorSummaryOptions>(o => actualOptions = o);

        var tagHelper = new ErrorSummaryTagHelper(componentGeneratorMock.Object)
        {
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.NotNull(actualOptions.ErrorList);
        Assert.Collection(
            actualOptions.ErrorList,
            error =>
            {
                Assert.Equal(itemErrorHref, error.Href);
                Assert.Equal(itemErrorHtml, error.Html);
            });
        Assert.True(containerErrorContext.ErrorSummaryHasBeenRendered);
    }

    [Fact]
    public async Task ProcessAsync_DoesNotHaveExplicitItemsGetsErrorsFromContainerErrorContext()
    {
        // Arrange
        var containerErrorContextErrorHtml = "First error";
        var containerErrorContextErrorHref = "#FirstError";

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
                var errorSummaryContext = (ErrorSummaryContext)context.Items[typeof(ErrorSummaryContext)];

                errorSummaryContext.SetTitle(new AttributeCollection(), "Title");
                errorSummaryContext.SetDescription(new AttributeCollection(), "Description");

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var viewContext = TestUtils.CreateViewContext();
        var containerErrorContext = viewContext.HttpContext.GetContainerErrorContext();
        containerErrorContext.AddError(containerErrorContextErrorHtml, containerErrorContextErrorHref);

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        ErrorSummaryOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateErrorSummary(It.IsAny<ErrorSummaryOptions>())).Callback<ErrorSummaryOptions>(o => actualOptions = o);

        var tagHelper = new ErrorSummaryTagHelper(componentGeneratorMock.Object)
        {
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.NotNull(actualOptions.ErrorList);
        Assert.Collection(
            actualOptions.ErrorList,
            error =>
            {
                Assert.Equal(containerErrorContextErrorHref, error.Href);
                Assert.Equal(containerErrorContextErrorHtml, error.Html);
            });
        Assert.True(containerErrorContext.ErrorSummaryHasBeenRendered);
    }

    [Fact]
    public async Task ProcessAsync_DoesNotHaveTitleOrDescriptionOrItemsRendersNothing()
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

        var viewContext = TestUtils.CreateViewContext();
        var containerErrorContext = viewContext.HttpContext.GetContainerErrorContext();

        var tagHelper = new ErrorSummaryTagHelper(new DefaultComponentGenerator())
        {
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Null(output.TagName);
        Assert.False(containerErrorContext.ErrorSummaryHasBeenRendered);
    }
}
