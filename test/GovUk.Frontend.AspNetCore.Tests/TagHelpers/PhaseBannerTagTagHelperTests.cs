using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PhaseBannerTagTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsToContext()
    {
        // Arrange
        var legendHtml = "Legend content";
        var phaseBannerContext = new PhaseBannerContext();

        var context = new TagHelperContext(
            tagName: "govuk-phase-banner-tag",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(PhaseBannerContext), phaseBannerContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-phase-banner-tag",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(legendHtml);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new PhaseBannerTagTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(legendHtml, phaseBannerContext.Tag?.Html);
    }

    [Fact]
    public async Task ProcessAsync_TagAlreadySpecified_ThrowsInvalidOperationException()
    {
        // Arrange
        var phaseBannerContext = new PhaseBannerContext();
        phaseBannerContext.SetTag(ImmutableDictionary<string, string?>.Empty, html: "Existing tag");

        var context = new TagHelperContext(
            tagName: "govuk-phase-banner-tag",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(PhaseBannerContext), phaseBannerContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-phase-banner-tag",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Legend message");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new PhaseBannerTagTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-phase-banner-tag> element is permitted within each <govuk-phase-banner>.", ex.Message);
    }
}
