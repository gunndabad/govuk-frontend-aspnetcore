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
        var prefixTagName = ShortTagNames.Prefix;
        var errorMessageTagName = ShortTagNames.ErrorMessage;
        context.SetPrefix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix", prefixTagName);

        // Act
        var ex = Record.Exception(
            () =>
                context.SetErrorMessage(null, ImmutableDictionary<string, string?>.Empty, "Error", errorMessageTagName)
        );

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{errorMessageTagName}> must be specified before <{prefixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetErrorMessage_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var suffixTagName = ShortTagNames.Suffix;
        var errorMessageTagName = ShortTagNames.ErrorMessage;
        context.SetSuffix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix", suffixTagName);

        // Act
        var ex = Record.Exception(
            () =>
                context.SetErrorMessage(null, ImmutableDictionary<string, string?>.Empty, "Error", errorMessageTagName)
        );

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{errorMessageTagName}> must be specified before <{suffixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetHint_AlreadyGotPrefix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var prefixTagName = ShortTagNames.Prefix;
        var hintTagName = ShortTagNames.Hint;
        context.SetPrefix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix", prefixTagName);

        // Act
        var ex = Record.Exception(
            () => context.SetHint(ImmutableDictionary<string, string?>.Empty, "Error", hintTagName)
        );

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{hintTagName}> must be specified before <{prefixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetHint_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var suffixTagName = ShortTagNames.Suffix;
        var hintTagName = ShortTagNames.ErrorMessage;
        context.SetSuffix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix", suffixTagName);

        // Act
        var ex = Record.Exception(
            () => context.SetHint(ImmutableDictionary<string, string?>.Empty, "Error", hintTagName)
        );

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{hintTagName}> must be specified before <{suffixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetLabel_AlreadyGotPrefix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var prefixTagName = ShortTagNames.Prefix;
        var labelTagName = ShortTagNames.Label;
        context.SetPrefix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix", prefixTagName);

        // Act
        var ex = Record.Exception(
            () => context.SetLabel(false, ImmutableDictionary<string, string?>.Empty, "Error", labelTagName)
        );

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{labelTagName}> must be specified before <{prefixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetLabel_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var suffixTagName = ShortTagNames.Suffix;
        var labelTagName = ShortTagNames.Label;
        context.SetSuffix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Prefix", suffixTagName);

        // Act
        var ex = Record.Exception(
            () => context.SetLabel(false, ImmutableDictionary<string, string?>.Empty, "Error", labelTagName)
        );

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{labelTagName}> must be specified before <{suffixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetPrefix_AlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var prefixTagName = ShortTagNames.Prefix;
        context.SetPrefix(
            attributes: ImmutableDictionary<string, string?>.Empty,
            html: "Existing prefix",
            prefixTagName
        );

        // Act
        var ex = Record.Exception(
            () => context.SetPrefix(ImmutableDictionary<string, string?>.Empty, "Prefix", prefixTagName)
        );

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal(
            $"Only one <prefix> or <govuk-input-prefix> element is permitted within each <govuk-input>.",
            ex.Message
        );
    }

    [Fact]
    public void SetPrefix_AlreadyGotSuffix_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var prefixTagName = ShortTagNames.Prefix;
        var suffixTagName = ShortTagNames.Suffix;
        context.SetSuffix(attributes: ImmutableDictionary<string, string?>.Empty, html: "Suffix", suffixTagName);

        // Act
        var ex = Record.Exception(
            () => context.SetPrefix(ImmutableDictionary<string, string?>.Empty, "Prefix", prefixTagName)
        );

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal($"<{prefixTagName}> must be specified before <{suffixTagName}>.", ex.Message);
    }

    [Fact]
    public void SetSuffix_AlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TextInputContext();
        var suffixTagName = ShortTagNames.Suffix;
        context.SetSuffix(
            attributes: ImmutableDictionary<string, string?>.Empty,
            html: "Existing prefix",
            suffixTagName
        );

        // Act
        var ex = Record.Exception(
            () => context.SetSuffix(ImmutableDictionary<string, string?>.Empty, "Prefix", suffixTagName)
        );

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal(
            $"Only one <suffix> or <govuk-input-suffix> element is permitted within each <govuk-input>.",
            ex.Message
        );
    }
}
