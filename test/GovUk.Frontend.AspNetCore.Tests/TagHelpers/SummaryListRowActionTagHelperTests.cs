using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class SummaryListRowActionTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsActionToContext()
    {
        // Arrange
        var summaryListContext = new SummaryListContext();

        var rowContext = new SummaryListRowContext();

        var context = new TagHelperContext(
            tagName: "govuk-summary-list-row-action",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(SummaryListContext), summaryListContext },
                { typeof(SummaryListRowContext), rowContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-summary-list-row-actions",
            attributes: new TagHelperAttributeList()
            {
                { "href", "#" }
            },
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Change");
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new SummaryListRowActionTagHelper()
        {
            VisuallyHiddenText = "vht"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            rowContext.Actions,
            action =>
            {
                Assert.Equal("Change", action.Content?.ToHtmlString());
                Assert.Equal("vht", action.VisuallyHiddenText);

                Assert.Collection(
                    action.Attributes,
                    kvp =>
                    {
                        Assert.Equal("href", kvp.Key);
                        Assert.Equal("#", kvp.Value);
                    });
            });
    }
}
