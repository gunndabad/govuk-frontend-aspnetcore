using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class CheckboxesItemHintTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsHintOnContext()
        {
            // Arrange
            var checkboxesItemContext = new CheckboxesItemContext();

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item-hint",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesItemContext), checkboxesItemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item-hint",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Hint");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemHintTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Hint", checkboxesItemContext.Hint?.Content?.RenderToString());
        }
    }
}
