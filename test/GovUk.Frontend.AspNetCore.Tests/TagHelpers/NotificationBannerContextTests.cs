using System;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class NotificationBannerContextTests
{
    [Fact]
    public void SetTitle_AlreadyGotTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new NotificationBannerContext();
        context.SetTitle("id", headingLevel: 4, content: new HtmlString("Title"));

        // Act
        var ex = Record.Exception(() => context.SetTitle("id", headingLevel: 4, content: new HtmlString("Title")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-notification-banner-title> element is permitted within each <govuk-notification-banner>.", ex.Message);
    }
}
