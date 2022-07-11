using System;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class RadiosContextTests
    {
        [Fact]
        public void AddItem_AddsItemToItems()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            var item = new RadiosItem()
            {
                LabelContent = new HtmlString("Item 1"),
                Value = "item1"
            };

            // Act
            context.AddItem(item);

            // Assert
            var contextItem = Assert.Single(context.Items);
            Assert.Same(item, contextItem);
        }

        [Fact]
        public void AddItem_OutsideOfFieldset_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            var item = new RadiosItem()
            {
                LabelContent = new HtmlString("Item 1"),
                Value = "item1"
            };

            context.OpenFieldset();
            var fieldsetContext = new RadiosFieldsetContext(attributes: null);
            context.CloseFieldset(fieldsetContext);

            // Act
            var ex = Record.Exception(() => context.AddItem(item));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-radios-item> must be inside <govuk-radios-fieldset>.", ex.Message);
        }

        [Fact]
        public void OpenFieldset_AlreadyOpen_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            context.OpenFieldset();

            // Act
            var ex = Record.Exception(() => context.OpenFieldset());

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-radios-fieldset> cannot be nested inside another <govuk-radios-fieldset>.", ex.Message);
        }

        [Fact]
        public void OpenFieldset_AlreadyGotFieldset_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            context.OpenFieldset();
            context.CloseFieldset(new RadiosFieldsetContext(attributes: null));

            // Act
            var ex = Record.Exception(() => context.OpenFieldset());

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal<object>("Only one <govuk-radios-fieldset> element is permitted within each <govuk-radios>.", ex.Message);
        }

        [Fact]
        public void OpenFieldset_AlreadyGotItem_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            var item = new RadiosItem()
            {
                LabelContent = new HtmlString("Item 1"),
                Value = "item1"
            };

            context.AddItem(item);

            // Act
            var ex = Record.Exception(() => context.OpenFieldset());

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-radios-fieldset> must be the only direct child of the <govuk-radios>.", ex.Message);
        }

        [Fact]
        public void OpenFieldset_AlreadyGotHint_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);
            context.SetHint(attributes: null, content: new HtmlString("Hint"));

            // Act
            var ex = Record.Exception(() => context.OpenFieldset());

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-radios-fieldset> must be the only direct child of the <govuk-radios>.", ex.Message);
        }

        [Fact]
        public void OpenFieldset_AlreadyGotErrorMessage_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);
            context.SetErrorMessage(visuallyHiddenText: null, attributes: null, content: new HtmlString("Error"));

            // Act
            var ex = Record.Exception(() => context.OpenFieldset());

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-radios-fieldset> must be the only direct child of the <govuk-radios>.", ex.Message);
        }

        [Fact]
        public void CloseFieldset_FieldsetNotOpened_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            // Act
            var ex = Record.Exception(() => context.CloseFieldset(new RadiosFieldsetContext(attributes: null)));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Fieldset has not been opened.", ex.Message);
        }

        [Fact]
        public void SetErrorMessage_AlreadyGotItem_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            var item = new RadiosItem()
            {
                LabelContent = new HtmlString("Item 1"),
                Value = "item1"
            };

            context.AddItem(item);

            // Act
            var ex = Record.Exception(
                () => context.SetErrorMessage(visuallyHiddenText: null, attributes: null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-radios-error-message> must be specified before <govuk-radios-item>.", ex.Message);
        }

        [Fact]
        public void SetErrorMessage_OutsideOfFieldset_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            var item = new RadiosItem()
            {
                LabelContent = new HtmlString("Item 1"),
                Value = "item1"
            };

            context.OpenFieldset();
            var fieldsetContext = new RadiosFieldsetContext(attributes: null);
            context.CloseFieldset(fieldsetContext);

            // Act
            var ex = Record.Exception(
                () => context.SetErrorMessage(visuallyHiddenText: null, attributes: null, new HtmlString("Error")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-radios-error-message> must be inside <govuk-radios-fieldset>.", ex.Message);
        }

        [Fact]
        public void SetHint_AlreadyGotItem_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            var item = new RadiosItem()
            {
                LabelContent = new HtmlString("Item 1"),
                Value = "item1"
            };

            context.AddItem(item);

            // Act
            var ex = Record.Exception(() => context.SetHint(attributes: null, new HtmlString("Hint")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-radios-hint> must be specified before <govuk-radios-item>.", ex.Message);
        }

        [Fact]
        public void SetHint_OutsideOfFieldset_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            var item = new RadiosItem()
            {
                LabelContent = new HtmlString("Item 1"),
                Value = "item1"
            };

            context.OpenFieldset();
            var fieldsetContext = new RadiosFieldsetContext(attributes: null);
            context.CloseFieldset(fieldsetContext);

            // Act
            var ex = Record.Exception(() => context.SetHint(attributes: null, new HtmlString("Hint")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-radios-hint> must be inside <govuk-radios-fieldset>.", ex.Message);
        }

        [Fact]
        public void SetLabel_ThrowsNotSupportedException()
        {
            // Arrange
            var context = new RadiosContext(name: null, aspFor: null);

            // Act
            var ex = Record.Exception(() => context.SetLabel(isPageHeading: false, attributes: null, content: null));

            // Assert
            Assert.IsType<NotSupportedException>(ex);
        }
    }
}
