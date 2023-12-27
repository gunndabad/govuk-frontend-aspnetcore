using System;
using System.Collections.Immutable;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class TextInputContextTests
{
    [Fact]
    public void SetErrorMessage_AlreadyGotPrefix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();

        context.SetPrefix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix");

        // Act
        var ex = Record.Exception(() => context.SetErrorMessage(null, ImmutableDictionary<string, string?>.Empty, "Error"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-input-error-message> must be specified before <govuk-input-prefix>.", ex.Message);
    }

    [Fact]
    public void SetErrorMessage_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();

        context.SetSuffix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix");

        // Act
        var ex = Record.Exception(() => context.SetErrorMessage(null, ImmutableDictionary<string, string?>.Empty, "Error"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-input-error-message> must be specified before <govuk-input-suffix>.", ex.Message);
    }

    [Fact]
    public void SetHint_AlreadyGotPrefix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();

        context.SetPrefix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix");

        // Act
        var ex = Record.Exception(() => context.SetHint(ImmutableDictionary<string, string?>.Empty, "Error"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-input-hint> must be specified before <govuk-input-prefix>.", ex.Message);
    }

    [Fact]
    public void SetHint_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();

        context.SetSuffix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix");

        // Act
        var ex = Record.Exception(() => context.SetHint(ImmutableDictionary<string, string?>.Empty, "Error"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-input-hint> must be specified before <govuk-input-suffix>.", ex.Message);
    }

    [Fact]
    public void SetLabel_AlreadyGotPrefix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();

        context.SetPrefix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix");

        // Act
        var ex = Record.Exception(() => context.SetLabel(false, ImmutableDictionary<string, string?>.Empty, "Error"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-input-label> must be specified before <govuk-input-prefix>.", ex.Message);
    }

    [Fact]
    public void SetLabel_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();

        context.SetSuffix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix");

        // Act
        var ex = Record.Exception(() => context.SetLabel(false, ImmutableDictionary<string, string?>.Empty, "Error"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-input-label> must be specified before <govuk-input-suffix>.", ex.Message);
    }

    [Fact]
    public void SetPrefix_AlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();

        context.SetPrefix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Existing prefix");

        // Act
        var ex = Record.Exception(() => context.SetPrefix(ImmutableDictionary<string, string?>.Empty, "Prefix"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-input-prefix> element is permitted within each <govuk-input>.", ex.Message);
    }

    [Fact]
    public void SetPrefix_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();

        context.SetSuffix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Suffix");

        // Act
        var ex = Record.Exception(() => context.SetPrefix(ImmutableDictionary<string, string?>.Empty, "Prefix"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<govuk-input-prefix> must be specified before <govuk-input-suffix>.", ex.Message);
    }

    [Fact]
    public void SetSuffix_AlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();

        context.SetSuffix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Existing prefix");

        // Act
        var ex = Record.Exception(() => context.SetSuffix(ImmutableDictionary<string, string?>.Empty, "Prefix"));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <govuk-input-suffix> element is permitted within each <govuk-input>.", ex.Message);
    }
}
