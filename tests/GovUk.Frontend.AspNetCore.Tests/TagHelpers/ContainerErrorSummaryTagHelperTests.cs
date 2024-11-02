using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ContainerErrorSummaryTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var options = Options.Create(new GovUkFrontendAspNetCoreOptions() { PrependErrorSummaryToForms = true });

        var disableAutoFocus = true;
        var errors = new (string Html, string? Href)[] { ("First error", "#Field1"), ("Second error", "#Field2") };

        var context = new TagHelperContext(
            tagName: "form",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "form",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var containerErrorContext = context.GetContextItem<ContainerErrorContext>();

                foreach (var error in errors)
                {
                    containerErrorContext.AddError(error.Html, error.Href);
                }

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        ErrorSummaryOptions? actualOptions = null;
        componentGeneratorMock
            .Setup(mock => mock.GenerateErrorSummary(It.IsAny<ErrorSummaryOptions>()))
            .Callback<ErrorSummaryOptions>(o => actualOptions = o);

        var tagHelper = new ContainerErrorSummaryTagHelper(componentGeneratorMock.Object)
        {
            DisableAutoFocus = disableAutoFocus,
            PrependErrorSummary = true,
            ViewContext = new ViewContext(),
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
        Assert.NotNull(actualOptions.ErrorList);
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
            }
        );
        Assert.Null(actualOptions.Classes);
        Assert.Null(actualOptions.Attributes);
        Assert.Equal(disableAutoFocus, actualOptions.DisableAutoFocus);
        Assert.Null(actualOptions.TitleAttributes);
        Assert.Null(actualOptions.DescriptionAttributes);
    }
}
