using System;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class RadiosItemContextTests
    {
        [Fact]
        public void SetConditional_SetsConditionalOnContext()
        {
            // Arrange
            var context = new RadiosItemContext();

            // Act
            context.SetConditional(attributes: null, content: new HtmlString("Conditional"));

            // Assert
            Assert.Equal("Conditional", context.Conditional?.Content?.ToString());
        }

        [Fact]
        public void SetConditional_AlreadyGotConditional_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosItemContext();
            context.SetConditional(attributes: null, content: new HtmlString("Existing conditional"));

            // Act
            var ex = Record.Exception(() => context.SetConditional(attributes: null, content: new HtmlString("Conditional")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal<object>("Only one <govuk-radios-item-conditional> element is permitted within each <govuk-radios-item>.", ex.Message);
        }

        [Fact]
        public void SetHint_SetsHintOnContext()
        {
            // Arrange
            var context = new RadiosItemContext();

            // Act
            context.SetHint(attributes: null, content: new HtmlString("Hint"));

            // Assert
            Assert.Equal("Hint", context.Hint?.Content?.ToString());
        }

        [Fact]
        public void SetHint_AlreadyGotConditional_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosItemContext();
            context.SetConditional(attributes: null, content: new HtmlString("Existing conditional"));

            // Act
            var ex = Record.Exception(() => context.SetHint(attributes: null, content: new HtmlString("Hint")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal<object>("<govuk-radios-item-hint> must be specified before <govuk-radios-item-conditional>.", ex.Message);
        }

        [Fact]
        public void SetHint_AlreadyGotHint_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosItemContext();
            context.SetHint(attributes: null, content: new HtmlString("Existing hint"));

            // Act
            var ex = Record.Exception(() => context.SetHint(attributes: null, content: new HtmlString("Hint")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal<object>("Only one <govuk-radios-item-hint> element is permitted within each <govuk-radios-item>.", ex.Message);
        }
    }
}
