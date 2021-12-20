using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class CheckboxesItemConditionalTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsConditionalOnContext()
        {
            // Arrange
            var checkboxesItemContext = new CheckboxesItemContext();

            var context = new TagHelperContext(
                tagName: "govuk-checkboxes-item-Conditional",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(CheckboxesItemContext), checkboxesItemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-checkboxes-item-Conditional",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Conditional");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new CheckboxesItemConditionalTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Conditional", checkboxesItemContext.Conditional?.Content?.RenderToString());
        }
    }
}
