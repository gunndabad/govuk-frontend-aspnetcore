using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class DateInputItemLabelTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsLabelOnContext()
        {
            // Arrange
            var itemContext = new DateInputItemContext("govuk-date-input-day", "govuk-date-input-day-label");

            var context = new TagHelperContext(
                tagName: "govuk-date-input-day-label",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(DateInputItemContext), itemContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-date-input-day-label",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Label");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new DateInputItemLabelTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("Label", itemContext.Label?.Content?.ToString());
        }
    }
}
