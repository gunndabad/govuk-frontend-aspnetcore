using System;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class ErrorSummaryContextTests
    {
        [Fact]
        public void AddItem_AddsItemToItems()
        {
            // Arrange
            var context = new ErrorSummaryContext();

            var item = new ErrorSummaryItem()
            {
                Content = new HtmlString("An error message"),
                Href = "#TheField"
            };

            // Act
            context.AddItem(item);

            // Assert
            Assert.Collection(
                context.Items,
                item =>
                {
                    Assert.Equal("An error message", item.Content.RenderToString());
                    Assert.Equal("#TheField", item.Href);
                });
        }

        [Fact]
        public void SetDescription_SetsDescriptionOnContext()
        {
            // Arrange
            var context = new ErrorSummaryContext();

            // Act
            context.SetDescription(attributes: null, content: new HtmlString("Description"));

            // Assert
            Assert.Equal("Description", context.Description?.Content?.RenderToString());
        }

        [Fact]
        public void SetDescription_AlreadyGotDescription_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new ErrorSummaryContext();
            context.SetDescription(attributes: null, content: new HtmlString("Existing description"));

            // Act
            var ex = Record.Exception(() => context.SetDescription(attributes: null, content: new HtmlString("Description")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-error-summary-description> element is permitted within each <govuk-error-summary>.", ex.Message);
        }

        [Fact]
        public void SetTitle_SetsTitleOnContext()
        {
            // Arrange
            var context = new ErrorSummaryContext();

            // Act
            context.SetTitle(attributes: null, content: new HtmlString("Title"));

            // Assert
            Assert.Equal("Title", context.Title?.Content?.RenderToString());
        }

        [Fact]
        public void SetTitle_AlreadyGotTitle_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new ErrorSummaryContext();
            context.SetTitle(attributes: null, content: new HtmlString("Existing title"));

            // Act
            var ex = Record.Exception(() => context.SetTitle(attributes: null, content: new HtmlString("Title")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-error-summary-title> element is permitted within each <govuk-error-summary>.", ex.Message);
        }
    }
}
