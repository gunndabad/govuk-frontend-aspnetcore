using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class FormErrorContextTests
{
    [Fact]
    public void AddError_AddsErrorToContext()
    {
        // Arrange
        var context = new ContainerErrorContext();
        var content = new HtmlString("Content");
        var href = new HtmlString("/foo");

        // Act
        context.AddError(content, href);

        // Assert
        Assert.Collection(
            context.Errors,
            item =>
            {
                Assert.Equal(content, item.Content);
                Assert.Equal(href, item.Href);
            });
    }
}
