using System;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class SummaryCardContextTests
{
    [Fact]
    public void SetTitle_AlreadyGotTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new SummaryCardContext();
        context.SetTitle(new HtmlString("Existing title"), headingLevel: 3, attributes: new AttributeDictionary());

        // Act
        var ex = Record.Exception(() => context.SetTitle(new HtmlString("New title"), headingLevel: 3, attributes: new AttributeDictionary()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-summary-card-title> element is permitted within each <govuk-summary-card>.", ex.Message);
    }

    [Fact]
    public void SetTitle_AlreadyGotActionsAttributes_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new SummaryCardContext();
        context.SetActionsAttributes(new AttributeDictionary());

        // Act
        var ex = Record.Exception(() => context.SetTitle(new HtmlString("New title"), headingLevel: 3, attributes: new AttributeDictionary()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-summary-card-title> must be specified before <govuk-summary-card-actions>.", ex.Message);
    }

    [Fact]
    public void SetTitle_AlreadyGotSummaryList_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new SummaryCardContext();
        context.SetSummaryList(new HtmlString("<div></div>"));

        // Act
        var ex = Record.Exception(() => context.SetTitle(new HtmlString("New title"), headingLevel: 3, attributes: new AttributeDictionary()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-summary-card-title> must be specified before <govuk-summary-list>.", ex.Message);
    }

    [Fact]
    public void AddAction_AlreadyGotSummaryList_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new SummaryCardContext();
        context.SetSummaryList(new HtmlString("<div></div>"));

        // Act
        var ex = Record.Exception(() => context.AddAction(new HtmlGeneration.SummaryListAction()
        {
            Attributes = new AttributeDictionary(),
            Content = new HtmlString("Action"),
            VisuallyHiddenText = "vht"
        }));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-summary-card-action> must be specified before <govuk-summary-list>.", ex.Message);
    }

    [Fact]
    public void SetActionsAttributes_AlreadyGotActionsAttributes_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new SummaryCardContext();
        context.SetActionsAttributes(new AttributeDictionary());

        // Act
        var ex = Record.Exception(() => context.SetActionsAttributes(new AttributeDictionary()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-summary-card-actions> element is permitted within each <govuk-summary-card>.", ex.Message);
    }

    [Fact]
    public void SetActionsAttributes_AlreadyGotAction_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new SummaryCardContext();
        context.AddAction(new HtmlGeneration.SummaryListAction()
        {
            Attributes = new AttributeDictionary(),
            Content = new HtmlString("Action"),
            VisuallyHiddenText = "vht"
        });

        // Act
        var ex = Record.Exception(() => context.SetActionsAttributes(new AttributeDictionary()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-summary-card-actions> must be specified before <govuk-summary-card-action>.", ex.Message);
    }

    [Fact]
    public void SetActionsAttributes_AlreadyGotSummaryList_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new SummaryCardContext();
        context.SetSummaryList(new HtmlString("<div></div>"));

        // Act
        var ex = Record.Exception(() => context.SetActionsAttributes(new AttributeDictionary()));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-summary-card-actions> must be specified before <govuk-summary-list>.", ex.Message);
    }

    [Fact]
    public void SetSummaryList_AlreadyGotSummaryList_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new SummaryCardContext();
        context.SetSummaryList(new HtmlString("<div></div>"));

        // Act
        var ex = Record.Exception(() => context.SetSummaryList(new HtmlString("<div></div>")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-summary-list> element is permitted within each <govuk-summary-card>.", ex.Message);
    }
}
