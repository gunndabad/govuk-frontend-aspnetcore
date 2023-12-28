using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class FormErrorSummaryTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions()
        {
            PrependErrorSummaryToForms = true
        });

        var disableAutoFocus = true;
        var errors = new (string Html, string? Href)[]
        {
            ("First error", "#Field1"),
            ("Second error", "#Field2"),
        };

        var context = new TagHelperContext(
            tagName: "form",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "form",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var formErrorContext = context.GetContextItem<FormErrorContext>();

                foreach (var error in errors)
                {
                    formErrorContext.AddError(error.Html, error.Href);
                }

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        ErrorSummaryOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateErrorSummary(It.IsAny<ErrorSummaryOptions>())).Callback<ErrorSummaryOptions>(o => actualOptions = o);

        var tagHelper = new FormErrorSummaryTagHelper(componentGeneratorMock.Object, options)
        {
            DisableAutoFocus = disableAutoFocus,
            ViewContext = new ViewContext()
        };

        tagHelper.Init(context);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Null(actualOptions!.TitleText);
        Assert.Null(actualOptions.TitleHtml);
        Assert.Null(actualOptions.DescriptionText);
        Assert.Null(actualOptions.DescriptionHtml);
        Assert.Collection(
            actualOptions.ErrorList,
            item =>
            {
                Assert.Null(item.Text);
                Assert.Equal(errors[0].Html, item.Html);
                Assert.Equal(errors[0].Href, item.Href);
                Assert.Null(item.Attributes);
            },
            item =>
            {
                Assert.Null(item.Text);
                Assert.Equal(errors[1].Html, item.Html);
                Assert.Equal(errors[1].Href, item.Href);
                Assert.Null(item.Attributes);
            });
        Assert.Null(actualOptions.Classes);
        Assert.Null(actualOptions.Attributes);
        Assert.Equal(disableAutoFocus, actualOptions.DisableAutoFocus);
        Assert.Null(actualOptions.TitleAttributes);
        Assert.Null(actualOptions.DescriptionAttributes);
    }

    [Theory]
    [InlineData(false, null, false)]
    [InlineData(false, false, false)]
    [InlineData(false, true, true)]
    [InlineData(true, null, true)]
    [InlineData(true, false, false)]
    [InlineData(true, true, true)]
    public async Task ProcessAsync_GeneratesExpectedOutput(
        bool prepentErrorSummaryToFormsOption,
        bool? prependErrorSummary,
        bool expectErrorSummary)
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions()
        {
            PrependErrorSummaryToForms = prepentErrorSummaryToFormsOption
        });

        var context = new TagHelperContext(
            tagName: "form",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "form",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var formErrorContext = (FormErrorContext)context.Items[typeof(FormErrorContext)];
                formErrorContext.AddError("Content", "href");

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new FormErrorSummaryTagHelper(new DefaultComponentGenerator(), options)
        {
            PrependErrorSummary = prependErrorSummary,
            ViewContext = new ViewContext()
        };

        tagHelper.Init(context);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var html = output.RenderToElement();
        Assert.Equal(expectErrorSummary ? 1 : 0, html.ChildElementCount);
    }
}
