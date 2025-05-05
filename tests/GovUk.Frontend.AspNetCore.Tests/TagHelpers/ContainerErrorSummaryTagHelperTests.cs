using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ContainerErrorSummaryTagHelperTests
{
    [Theory]
    [InlineData(false, null, false)]
    [InlineData(false, false, false)]
    [InlineData(false, true, true)]
    [InlineData(true, null, true)]
    [InlineData(true, false, false)]
    [InlineData(true, true, true)]
    public async Task ProcessAsync_RendersWhenExpected(
        bool prepentErrorSummaryToFormsOption,
        bool? prependErrorSummary,
        bool expectErrorSummary)
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions()
        {
            PrependErrorSummaryToForms = prepentErrorSummaryToFormsOption
        });

        var errorHtml = "Error message";
        var errorHref = "#Field";

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
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        ErrorSummaryOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GenerateErrorSummary(It.IsAny<ErrorSummaryOptions>())).Callback<ErrorSummaryOptions>(o => actualOptions = o);

        var viewContext = TestUtils.CreateViewContext();
        var containerErrorContext = viewContext.HttpContext.GetContainerErrorContext();
        containerErrorContext.AddError(errorHtml, errorHref);

        var tagHelper = new ContainerErrorSummaryTagHelper(componentGeneratorMock.Object, options)
        {
            PrependErrorSummary = prependErrorSummary,
            ViewContext = viewContext
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var html = output.RenderToElement();
        Assert.Equal(expectErrorSummary ? 1 : 0, html.ChildElementCount);

        if (expectErrorSummary)
        {
            Assert.NotNull(actualOptions);
            Assert.NotNull(actualOptions.ErrorList);

            Assert.Collection(
                actualOptions.ErrorList,
                error =>
                {
                    Assert.Equal(errorHref, error.Href);
                    Assert.Equal(errorHtml, error.Html);
                });
        }
    }
}
