using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PanelBodyTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsBodyOnContext()
    {
        // Arrange
        var panelContext = new PanelContext();

        var context = new TagHelperContext(
            tagName: "govuk-panel-body",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>() { { typeof(PanelContext), panelContext } },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-panel-body",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("The body");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new PanelBodyTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("The body", panelContext.Body?.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_ParentAlreadyHasBody_ThrowsInvalidOperationException()
    {
        // Arrange
        var panelContext = new PanelContext();
        panelContext.SetBody(new HtmlString("The body"));

        var context = new TagHelperContext(
            tagName: "govuk-panel-body",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>() { { typeof(PanelContext), panelContext } },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-panel-body",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("The body");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new PanelBodyTagHelper();

        // Act
        var ex = await Record.ExceptionAsync(() => tagHelper.ProcessAsync(context, output));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-panel-body> element is permitted within each <govuk-panel>.", ex.Message);
    }
}
