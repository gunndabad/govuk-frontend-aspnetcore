using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ErrorSummaryContextTests
{
    [Fact]
    public void AddItem_AddsItemToItems()
    {
        // Arrange
        var errorMessageHtml = "Error message";
        var href = "#TheField";

        var context = new ErrorSummaryContext();

        var item = new ErrorSummaryContextItem(
            href,
            errorMessageHtml,
            new AttributeCollection(),
            new AttributeCollection());

        // Act
        context.AddItem(item);

        // Assert
        Assert.Collection(
            context.Items,
            item =>
            {
                Assert.Equal(errorMessageHtml, item.Html);
                Assert.Equal(href, item.Href);
            });
    }

    [Fact]
    public void SetDescription_SetsDescriptionOnContext()
    {
        // Arrange
        var descriptionHtml = "Description";

        var context = new ErrorSummaryContext();

        // Act
        context.SetDescription(new AttributeCollection(), descriptionHtml);

        // Assert
        Assert.Equal(descriptionHtml, context.Description?.Html);
    }

    [Fact]
    public void SetDescription_AlreadyGotDescription_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new ErrorSummaryContext();
        context.SetDescription(new AttributeCollection(), html: "Existing description");

        // Act
        var ex = Record.Exception(() => context.SetDescription(new AttributeCollection(), html: "Description"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-error-summary-description> element is permitted within each <govuk-error-summary>.", ex.Message);
    }

    [Fact]
    public void SetTitle_SetsTitleOnContext()
    {
        // Arrange
        var titleHtml = "Title";

        var context = new ErrorSummaryContext();

        // Act
        context.SetTitle(new AttributeCollection(), titleHtml);

        // Assert
        Assert.Equal(titleHtml, context.Title?.Html);
    }

    [Fact]
    public void SetTitle_AlreadyGotTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new ErrorSummaryContext();
        context.SetTitle(new AttributeCollection(), html: "Existing title");

        // Act
        var ex = Record.Exception(() => context.SetTitle(new AttributeCollection(), html: "Title"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-error-summary-title> element is permitted within each <govuk-error-summary>.", ex.Message);
    }
}
