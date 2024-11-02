using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputErrorMessageTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_SetsErrorMessageAndErrorComponentsOnContext()
    {
        // Arrange
        var errorHtml = "Error message";
        var errorComponents = DateInputErrorFields.Day | DateInputErrorFields.Month;

        var dateInputContext = new DateInputContext(haveExplicitValue: false, @for: null);

        var context = new TagHelperContext(
            tagName: "govuk-date-input-error-message",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>()
            {
                { typeof(DateInputContext), dateInputContext }
            },
            uniqueId: "test");

        var output = new TagHelperOutput(
            "govuk-date-input-error-message",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(errorHtml);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var tagHelper = new DateInputErrorMessageTagHelper()
        {
            ErrorFields = errorComponents
        };

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(errorHtml, dateInputContext.ErrorMessage?.Html);
        Assert.Equal(errorComponents, dateInputContext.ErrorFields);
    }
}
