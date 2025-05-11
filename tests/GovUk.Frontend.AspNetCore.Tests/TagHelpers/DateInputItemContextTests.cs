using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputItemContextTests
{
    [Fact]
    public void SetLabel_SetsLabelOnContext()
    {
        // Arrange
        var context = new DateInputItemContext("govuk-date-input-day", "govuk-date-input-day-label");

        // Act
        context.SetLabel(content: new HtmlString("Label"), attributes: new AttributeDictionary());

        // Assert
        Assert.Equal("Label", context.Label?.Content?.ToHtmlString());
    }

    [Fact]
    public void SetLabel_AlreadyGotLabel_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputItemContext("govuk-date-input-day", "govuk-date-input-day-label");
        context.SetLabel(content: new HtmlString("Existing label"), attributes: new AttributeDictionary());

        // Act
        var ex = Record.Exception(() => context.SetLabel(content: new HtmlString("Label"), attributes: new AttributeDictionary()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-date-input-day-label> element is permitted within each <govuk-date-input-day>.", ex.Message);
    }
}
