using System;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class DateInputItemContextTests
{
    [Fact]
    public void SetLabel_SetsLabelOnContext()
    {
        // Arrange
        var labelHtml = "Label";
        var context = new DateInputItemContext("govuk-date-input-day", "govuk-date-input-day-label");

        // Act
        context.SetLabel(attributes: ImmutableDictionary<string, string?>.Empty, html: labelHtml);

        // Assert
        Assert.Equal(labelHtml, context.Label?.Html);
    }

    [Fact]
    public void SetLabel_AlreadyGotLabel_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new DateInputItemContext("govuk-date-input-day", "govuk-date-input-day-label");
        context.SetLabel(attributes: ImmutableDictionary<string, string?>.Empty, html: null);

        // Act
        var ex = Record.Exception(() => context.SetLabel(attributes: ImmutableDictionary<string, string?>.Empty, html: null));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-date-input-day-label> element is permitted within each <govuk-date-input-day>.", ex.Message);
    }
}
