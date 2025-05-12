using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PhaseBannerTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_GeneratesExpectedOutput()
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
                var pbContext = context.GetContextItem<PhaseBannerContext>();
                pbContext.SetTag(new AttributeDictionary(), content: new HtmlString("Alpha"));

                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Phase message");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new PhaseBannerTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-phase-banner"">
    <p class=""govuk-phase-banner__content"">
        <strong class=""govuk-phase-banner__content__tag govuk-tag"">Alpha</strong>
        <span class=""govuk-phase-banner__text"">Phase message</span>
    </p>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_MissingIconFallbackText_ThrowsInvalidOperationException()
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

        var tagHelper = new PhaseBannerTagHelper(new ComponentGenerator());

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("A <govuk-phase-banner-tag> element must be provided.", ex.Message);
    }
}
