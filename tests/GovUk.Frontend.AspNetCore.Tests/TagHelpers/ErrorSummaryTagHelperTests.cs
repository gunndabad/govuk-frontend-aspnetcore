using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        ErrorSummaryOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateErrorSummary(It.IsAny<ErrorSummaryOptions>())).Callback<ErrorSummaryOptions>(o => actualOptions = o);

        var tagHelper = new ErrorSummaryTagHelper(componentGeneratorMock.Object)
        {
            DisableAutoFocus = disableAutoFocus,
            ViewContext = TestUtils.CreateViewContext()
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

        var tagHelper = new ErrorSummaryTagHelper(new DefaultComponentGenerator());

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var html = output.ToHtmlString();
        Assert.Empty(html);
    }
}
