using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PanelTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_GeneratesExpectedOutput()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-panel",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-panel",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var panelContext = (PanelContext)context.Items[typeof(PanelContext)];
                panelContext.SetTitle(new HtmlString("Title"));
                panelContext.SetBody(new HtmlString("Body"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new PanelTagHelper(new ComponentGenerator())
        {
            HeadingLevel = 3
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = @"
<div class=""govuk-panel--confirmation govuk-panel"">
    <h3 class=""govuk-panel__title"">Title</h3>
    <div class=""govuk-panel__body"">Body</div>
</div>";

        AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_MissingTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TagHelperContext(
            tagName: "govuk-panel",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-panel",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var panelContext = (PanelContext)context.Items[typeof(PanelContext)];
                panelContext.SetBody(new HtmlString("Body"));

                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new PanelTagHelper(new ComponentGenerator())
        {
            HeadingLevel = 3
        };

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("A <govuk-panel-title> element must be provided.", ex.Message);
    }
}
