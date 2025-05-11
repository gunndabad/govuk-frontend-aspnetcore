using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class PanelContextTests
{
    [Fact]
    public void SetBody_AlreadyGotBody_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PanelContext();
        context.SetBody(new HtmlString("Body"));

        // Act
        var ex = Record.Exception(() => context.SetBody(new HtmlString("Body")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-panel-body> element is permitted within each <govuk-panel>.", ex.Message);
    }

    [Fact]
    public void SetTitle_AlreadyGotBody_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PanelContext();
        context.SetBody(new HtmlString("Body"));

        // Act
        var ex = Record.Exception(() => context.SetTitle(new HtmlString("Title")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-panel-title> must be specified before <govuk-panel-body>.", ex.Message);
    }

    [Fact]
    public void SetTitle_AlreadyGotTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new PanelContext();
        context.SetTitle(new HtmlString("Title"));

        // Act
        var ex = Record.Exception(() => context.SetTitle(new HtmlString("Title")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-panel-title> element is permitted within each <govuk-panel>.", ex.Message);
    }
}
