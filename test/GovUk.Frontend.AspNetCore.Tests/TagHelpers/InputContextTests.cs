using System;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class InputContextTests
    {
        [Fact]
        public void SetErrorMessage_AlreadyGotPrefix_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new InputContext();

            context.SetPrefix(attributes: null, content: new HtmlString("Prefix"));

            // Act
            var ex = Record.Exception(() => context.SetErrorMessage(null, null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-input-error-message> must be specified before <govuk-input-prefix>.", ex.Message);
        }

        [Fact]
        public void SetErrorMessage_AlreadyGotSuffix_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new InputContext();

            context.SetSuffix(attributes: null, content: new HtmlString("Prefix"));

            // Act
            var ex = Record.Exception(() => context.SetErrorMessage(null, null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-input-error-message> must be specified before <govuk-input-suffix>.", ex.Message);
        }

        [Fact]
        public void SetHint_AlreadyGotPrefix_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new InputContext();

            context.SetPrefix(attributes: null, content: new HtmlString("Prefix"));

            // Act
            var ex = Record.Exception(() => context.SetHint(null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-input-hint> must be specified before <govuk-input-prefix>.", ex.Message);
        }

        [Fact]
        public void SetHint_AlreadyGotSuffix_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new InputContext();

            context.SetSuffix(attributes: null, content: new HtmlString("Prefix"));

            // Act
            var ex = Record.Exception(() => context.SetHint(null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-input-hint> must be specified before <govuk-input-suffix>.", ex.Message);
        }

        [Fact]
        public void SetLabel_AlreadyGotPrefix_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new InputContext();

            context.SetPrefix(attributes: null, content: new HtmlString("Prefix"));

            // Act
            var ex = Record.Exception(() => context.SetLabel(false, null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-input-label> must be specified before <govuk-input-prefix>.", ex.Message);
        }

        [Fact]
        public void SetLabel_AlreadyGotSuffix_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new InputContext();

            context.SetSuffix(attributes: null, content: new HtmlString("Prefix"));

            // Act
            var ex = Record.Exception(() => context.SetLabel(false, null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-input-label> must be specified before <govuk-input-suffix>.", ex.Message);
        }

        [Fact]
        public void SetPrefix_AlreadySet_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new InputContext();

            context.SetPrefix(attributes: null, content: new HtmlString("Existing prefix"));

            // Act
            var ex = Record.Exception(() => context.SetPrefix(null, new HtmlString("Prefix")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-input-prefix> element is permitted within each <govuk-input>.", ex.Message);
        }

        [Fact]
        public void SetPrefix_AlreadyGotSuffix_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new InputContext();

            context.SetSuffix(attributes: null, content: new HtmlString("Suffix"));

            // Act
            var ex = Record.Exception(() => context.SetPrefix(null, new HtmlString("Prefix")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-input-prefix> must be specified before <govuk-input-suffix>.", ex.Message);
        }

        [Fact]
        public void SetSuffix_AlreadySet_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new InputContext();

            context.SetSuffix(attributes: null, content: new HtmlString("Existing prefix"));

            // Act
            var ex = Record.Exception(() => context.SetSuffix(null, new HtmlString("Prefix")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-input-suffix> element is permitted within each <govuk-input>.", ex.Message);
        }
    }
}
