using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class FormGroupContextTests
{
    [Fact]
    public void SetErrorMessage_SetsErrorMessageOnContext()
    {
        // Arrange
        var context = new TestFormGroupContext();

        // Act
        context.SetErrorMessage(visuallyHiddenText: "vht", null, new HtmlString("Error"));

        // Assert
        Assert.Equal("vht", context.ErrorMessage?.VisuallyHiddenText);
        Assert.Equal("Error", context.ErrorMessage?.Content?.ToString());
    }

    [Fact]
    public void SetErrorMessage_AlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TestFormGroupContext();

        context.SetErrorMessage(
            visuallyHiddenText: null,
            attributes: null,
            content: new HtmlString("Existing error"));

        // Act
        var ex = Record.Exception(() => context.SetErrorMessage(null, null, new HtmlString("Error")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <test-error-message> element is permitted within each <test>.", ex.Message);
    }

    [Fact]
    public void SetHint_SetsHintOnContext()
    {
        // Arrange
        var context = new TestFormGroupContext();

        // Act
        context.SetHint(null, new HtmlString("Hint"));

        // Assert
        Assert.Equal("Hint", context.Hint?.Content?.ToString());
    }

    [Fact]
    public void SetHint_AlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TestFormGroupContext();

        context.SetHint(
            attributes: null,
            content: new HtmlString("Existing hint"));

        // Act
        var ex = Record.Exception(() => context.SetHint(null, new HtmlString("Hint")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <test-hint> element is permitted within each <test>.", ex.Message);
    }

    [Fact]
    public void SetHint_ErrorMessageAlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TestFormGroupContext();

        context.SetErrorMessage(
            visuallyHiddenText: null,
            attributes: null,
            content: new HtmlString("Error message"));

        // Act
        var ex = Record.Exception(() => context.SetHint(null, new HtmlString("Hint")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<test-hint> must be specified before <test-error-message>.", ex.Message);
    }

    [Fact]
    public void SetLabel_SetsLabelOnContext()
    {
        // Arrange
        var context = new TestFormGroupContext();

        // Act
        context.SetLabel(isPageHeading: true, null, new HtmlString("Label"));

        // Assert
        Assert.True(context.Label?.IsPageHeading);
        Assert.Equal("Label", context.Label?.Content?.ToString());
    }

    [Fact]
    public void SetLabel_AlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TestFormGroupContext();

        context.SetLabel(
            isPageHeading: false,
            attributes: null,
            content: new HtmlString("Existing label"));

        // Act
        var ex = Record.Exception(() => context.SetLabel(false, null, new HtmlString("Label")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("Only one <test-label> element is permitted within each <test>.", ex.Message);
    }

    [Fact]
    public void SetLabel_ErrorMessageAlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TestFormGroupContext();

        context.SetErrorMessage(
            visuallyHiddenText: null,
            attributes: null,
            content: new HtmlString("Error message"));

        // Act
        var ex = Record.Exception(() => context.SetLabel(false, null, new HtmlString("Hint")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<test-label> must be specified before <test-error-message>.", ex.Message);
    }

    [Fact]
    public void SetLabel_HintMessageAlreadySet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new TestFormGroupContext();

        context.SetHint(
            attributes: null,
            content: new HtmlString("Hint"));

        // Act
        var ex = Record.Exception(() => context.SetLabel(false, null, new HtmlString("Hint")));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
        Assert.Equal("<test-label> must be specified before <test-hint>.", ex.Message);
    }

    private class TestFormGroupContext : FormGroupContext
    {
        protected override string ErrorMessageTagName => "test-error-message";

        protected override string HintTagName => "test-hint";

        protected override string LabelTagName => "test-label";

        protected override string RootTagName => "test";
    }
}
