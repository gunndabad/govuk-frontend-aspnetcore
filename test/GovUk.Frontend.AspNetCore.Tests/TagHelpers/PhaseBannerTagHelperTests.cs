using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PhaseBannerTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_InvokesComponentGeneratorWithExpectedOptions()
    {
        // Arrange
        var tagContent = "Alpha";
        var message = "Phase message";

        var context = new TagHelperContext(
            tagName: "govuk-phase-banner",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-phase-banner",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var pbContext = context.GetContextItem<PhaseBannerContext>();
                pbContext.SetTag(ImmutableDictionary<string, string?>.Empty, html: tagContent);

                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(message);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<DefaultComponentGenerator>() { CallBase = true };
        PhaseBannerOptions? actualOptions = null;
        componentGeneratorMock.Setup(mock => mock.GeneratePhaseBanner(It.IsAny<PhaseBannerOptions>())).Callback<PhaseBannerOptions>(o => actualOptions = o);

        var tagHelper = new PhaseBannerTagHelper(componentGeneratorMock.Object);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.NotNull(actualOptions);
        Assert.Equal(tagContent, actualOptions!.Tag?.Html);
        Assert.Equal(message, actualOptions!.Html);
    }

    [Fact]
    public async Task ProcessAsync_MissingTag_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-phase-banner",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-phase-banner",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Phase message");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var componentGeneratorMock = new Mock<IComponentGenerator>();

        var tagHelper = new PhaseBannerTagHelper(componentGeneratorMock.Object);

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("A <govuk-phase-banner-tag> element must be provided.", ex.Message);
    }
}
