using System;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class ErrorSummaryContextTests
{
    [Fact]
    public void AddItem_AddsItemToItems()
    {
        // Arrange
        var context = new ErrorSummaryContext();

        var item = new ErrorSummaryOptionsErrorItem() { Html = "An error message", Href = "#TheField" };

        // Act
        context.AddItem(item);

        // Assert
        Assert.Collection(
            context.Items,
            item =>
            {
                Assert.Equal("An error message", item.Html);
                Assert.Equal("#TheField", item.Href);
            }
        );
    }

    [Fact]
    public void SetDescription_SetsDescriptionOnContext()
    {
        // Arrange
        var context = new ErrorSummaryContext();

        // Act
        context.SetDescription(ImmutableDictionary<string, string?>.Empty, html: "Description");

        // Assert
        Assert.Equal("Description", context.Description?.Html);
    }

    [Fact]
    public void SetDescription_AlreadyGotDescription_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new ErrorSummaryContext();
        context.SetDescription(ImmutableDictionary<string, string?>.Empty, html: "Existing description");

        // Act
        var ex = Record.Exception(
            () => context.SetDescription(ImmutableDictionary<string, string?>.Empty, html: "Description")
        );

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal(
            "Only one <govuk-error-summary-description> element is permitted within each <govuk-error-summary>.",
            ex.Message
        );
    }

    [Fact]
    public void SetTitle_SetsTitleOnContext()
    {
        // Arrange
        var context = new ErrorSummaryContext();

        // Act
        context.SetTitle(ImmutableDictionary<string, string?>.Empty, html: "Title");

        // Assert
        Assert.Equal("Title", context.Title?.Html);
    }

    [Fact]
    public void SetTitle_AlreadyGotTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new ErrorSummaryContext();
        context.SetTitle(ImmutableDictionary<string, string?>.Empty, html: "Existing title");

        // Act
        var ex = Record.Exception(() => context.SetTitle(ImmutableDictionary<string, string?>.Empty, html: "Title"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal(
            "Only one <govuk-error-summary-title> element is permitted within each <govuk-error-summary>.",
            ex.Message
        );
    }
}
