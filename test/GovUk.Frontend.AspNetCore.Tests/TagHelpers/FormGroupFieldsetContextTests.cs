using System;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class FormGroupFieldsetContextTests
    {
        [Fact]
        public void SetLegend_SetsLegendOnContext()
        {
            // Arrange
            var context = new FormGroupFieldsetContext(
                fieldsetTagName: "test-fieldset",
                legendTagName: "test-fieldset-legend",
                attributes: null,
                describedBy: null);

            // Act
            context.SetLegend(isPageHeading: true, null, new HtmlString("Legend"));

            // Assert
            Assert.True(context.Legend?.IsPageHeading);
            Assert.Equal("Legend", context.Legend?.Content?.ToString());
        }

        [Fact]
        public void SetLegend_AlreadySet_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = new FormGroupFieldsetContext(
                fieldsetTagName: "test-fieldset",
                legendTagName: "test-fieldset-legend",
                attributes: null,
                describedBy: null);

            context.SetLegend(false, null, new HtmlString("Existing legend"));

            // Act
            var ex = Record.Exception(() => context.SetLegend(isPageHeading: true, null, new HtmlString("Legend")));

            // Assert
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Only one <test-fieldset-legend> element is permitted within each <test-fieldset>.", ex.Message);
        }
    }
}
