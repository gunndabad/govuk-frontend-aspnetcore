using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class InputPrefixTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsPrefixOnContext()
        {
            // Arrange
            var inputContext = new InputContext();

            var context = new TagHelperContext(
                tagName: "govuk-input-prefix",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(InputContext), inputContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-input-prefix",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.AppendHtml(new HtmlString("Prefix"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new InputPrefixTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(inputContext.Prefix.HasValue);
            Assert.Equal("Prefix", inputContext.Prefix.Value.Content.RenderToString());
        }
    }
}
