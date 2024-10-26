using GovUk.Frontend.AspNetCore.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public class FormErrorContextTests
{
    [Fact]
    public void AddError_AddsErrorToContext()
    {
        // Arrange
        var context = new ContainerErrorContext();
        var html = "Content";
        var href = "/foo";

        // Act
        context.AddError(html, href);

        // Assert
        Assert.Collection(
            context.Errors,
            item =>
            {
                Assert.Equal(html, item.Html);
                Assert.Equal(href, item.Href);
            });
    }
}
