using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class RadiosItemDividerTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_AddsDividerToContextItems()
    {
        // Arrange
        var radiosContext = new RadiosContext(name: null, aspFor: null);

        var context = new TagHelperContext(
            tagName: "govuk-radios-divider",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>() { { typeof(RadiosContext), radiosContext } },
            uniqueId: "test"
        );

        var output = new TagHelperOutput(
            "govuk-radios-divider",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.AppendHtml(new HtmlString("Divider"));
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }
        );

        var tagHelper = new RadiosItemDividerTagHelper();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Collection(
            radiosContext.Items,
            item =>
            {
                var divider = Assert.IsType<RadiosItemDivider>(item);
                Assert.Equal("Divider", divider.Content?.ToString());
            }
        );
    }
}
