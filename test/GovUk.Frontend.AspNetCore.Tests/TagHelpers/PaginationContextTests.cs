using System;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class PaginationContextTests
    {
        [Fact]
        public void AddItem_AlreadyGotNext_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new PaginationContext();
            context.SetNext(new PaginationNext());

            // Act
            var ex = Record.Exception(() => context.AddItem(new PaginationItemEllipsis()));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-pagination-ellipsis> must be specified before <govuk-pagination-next>.", ex.Message);
        }

        [Fact]
        public void AddItem_WithCurrentItemAndAlreadyGotCurrentItem_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new PaginationContext();
            context.AddItem(new PaginationItem()
            {
                Number = new HtmlString("1"),
                IsCurrent = true
            });

            // Act
            var ex = Record.Exception(() => context.AddItem(new PaginationItem()
            {
                Number = new HtmlString("2"),
                IsCurrent = true
            }));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one current govuk-pagination-item is permitted.", ex.Message);
        }

        [Fact]
        public void AddItem_ValidRequest_AddsItemToContext()
        {
            // Arrange
            var context = new PaginationContext();
            var item = new PaginationItemEllipsis();

            // Act
            context.AddItem(item);

            // Assert
            Assert.Collection(context.Items, i => Assert.Same(item, i));
        }

        [Fact]
        public void SetNext_AlreadyGotNext_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new PaginationContext();
            context.SetNext(new PaginationNext());

            // Act
            var ex = Record.Exception(() => context.SetNext(new PaginationNext()));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-pagination-next> element is permitted within each <govuk-pagination>.", ex.Message);
        }

        [Fact]
        public void SetNext_ValidRequest_SetsNextOnContext()
        {
            // Arrange
            var context = new PaginationContext();
            var next = new PaginationNext();

            // Act
            context.SetNext(next);

            // Assert
            Assert.Same(next, context.Next);
        }

        [Fact]
        public void SetPrevious_AlreadyGotNext_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new PaginationContext();
            context.SetNext(new PaginationNext());

            // Act
            var ex = Record.Exception(() => context.SetPrevious(new PaginationPrevious()));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-pagination-previous> must be specified before <govuk-pagination-next>.", ex.Message);
        }

        [Fact]
        public void SetPrevious_AlreadyGotPrevious_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new PaginationContext();
            context.SetPrevious(new PaginationPrevious());

            // Act
            var ex = Record.Exception(() => context.SetPrevious(new PaginationPrevious()));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <govuk-pagination-previous> element is permitted within each <govuk-pagination>.", ex.Message);
        }

        [Fact]
        public void SetPrevious_AlreadyGotItems_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new PaginationContext();
            context.AddItem(new PaginationItem()
            {
                Number = new HtmlString("1"),
                IsCurrent = true
            });

            // Act
            var ex = Record.Exception(() => context.SetPrevious(new PaginationPrevious()));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("<govuk-pagination-previous> must be specified before <govuk-pagination-item>.", ex.Message);
        }

        [Fact]
        public void SetPrevious_ValidRequest_SetsPreviousOnContext()
        {
            // Arrange
            var context = new PaginationContext();
            var previous = new PaginationPrevious();

            // Act
            context.SetPrevious(previous);

            // Assert
            Assert.Same(previous, context.Previous);
        }
    }
}
