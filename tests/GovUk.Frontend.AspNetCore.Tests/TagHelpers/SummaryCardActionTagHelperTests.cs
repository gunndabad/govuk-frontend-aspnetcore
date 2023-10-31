using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class SummaryCardActionTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsActionToContext()
    {
        // Arrange
        var summaryCardContext = new SummaryCardContext();

        var context = new TagHelperContext(
            tagName: "govuk-summary-card-action",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(SummaryCardContext), summaryCardContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-summary-card-action",
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

        var tagHelper = new SummaryCardActionTagHelper()
        {
            VisuallyHiddenText = "vht"
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            summaryCardContext.Actions,
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
