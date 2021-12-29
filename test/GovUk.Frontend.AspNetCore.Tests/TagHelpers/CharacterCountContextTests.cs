using System;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class CharacterCountContextTests
    {
        [Fact]
        public void SetErrorMessage_AlreadyGotValue_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new CharacterCountContext();

            context.SetValue(new HtmlString("Value"));

            // Act
            var ex = Record.Exception(() => context.SetErrorMessage(null, null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-character-count-error-message> must be specified before <govuk-character-count-value>.", ex.Message);
        }

        [Fact]
        public void SetHint_AlreadyGotValue_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new CharacterCountContext();

            context.SetValue(new HtmlString("Value"));

            // Act
            var ex = Record.Exception(() => context.SetHint(null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-character-count-hint> must be specified before <govuk-character-count-value>.", ex.Message);
        }

        [Fact]
        public void SetLabel_AlreadyGotValue_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new CharacterCountContext();

            context.SetValue(new HtmlString("Value"));

            // Act
            var ex = Record.Exception(() => context.SetLabel(false, null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-character-count-label> must be specified before <govuk-character-count-value>.", ex.Message);
        }

        [Fact]
        public void SetValue_AlreadySet_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new CharacterCountContext();

            context.SetValue(new HtmlString("Existing value"));

            // Act
            var ex = Record.Exception(() => context.SetValue(new HtmlString("Value")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-character-count-value> element is permitted within each <govuk-character-count>.", ex.Message);
        }
    }
}
