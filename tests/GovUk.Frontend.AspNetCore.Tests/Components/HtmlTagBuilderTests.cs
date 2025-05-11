using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Tests.Components;

public class HtmlTagBuilderTests
{
    [Fact]
    public void EmptyTag_WritesCorrectHtml()
    {
        // Arrange
        var builder = new HtmlTagBuilder("div");

        // Act
        var rendered = builder.ToString();

        // Assert
        Assert.Equal("<div></div>", rendered);
    }

    [Fact]
    public void EmptyVoidTag_WritesCorrectHtml()
    {
        // Arrange
        var builder = new HtmlTagBuilder("img");

        // Act
        var rendered = builder.ToString();

        // Assert
        Assert.Equal("<img>", rendered);
    }

    [Fact]
    public void TagWithAttributes_WritesCorrectHtml()
    {
        // Arrange
        var builder = new HtmlTagBuilder("a")
            .WithAttribute("class", "govuk-link", encodeValue: false)
            .WithAttribute("href", "page?foo=bar&baz=qux", encodeValue: false);

        // Act
        var rendered = builder.ToString();

        // Assert
        Assert.Equal("""
            <a class="govuk-link" href="page?foo=bar&baz=qux"></a>
            """,
            rendered);
    }

    [Fact]
    public void TagWithBooleanAttributes_WritesCorrectHtml()
    {
        // Arrange
        var builder = new HtmlTagBuilder("input")
            .WithBooleanAttribute("disabled");

        // Act
        var rendered = builder.ToString();

        // Assert
        Assert.Equal("<input disabled>", rendered);
    }

    [Fact]
    public void TagWithTextContent_WritesCorrectHtml()
    {
        // Arrange
        var builder = new HtmlTagBuilder("p")
            .WithAppendedText("Foo & bar");

        // Act
        var rendered = builder.ToString();

        // Assert
        Assert.Equal("<p>Foo &amp; bar</p>", rendered);
    }

    [Fact]
    public void TagWithHtmlContent_WritesCorrectHtml()
    {
        // Arrange
        var builder = new HtmlTagBuilder("p")
            .WithAppendedHtml("<span>Foo &amp; bar</span>");

        // Act
        var rendered = builder.ToString();

        // Assert
        Assert.Equal("<p><span>Foo &amp; bar</span></p>", rendered);
    }

    [Fact]
    public void TagWithContent_WritesCorrectHtml()
    {
        // Arrange
        var builder = new HtmlTagBuilder("p")
            .WithAppendedHtml(new HtmlString("<span>Foo &amp; bar</span>"));

        // Act
        var rendered = builder.ToString();

        // Assert
        Assert.Equal("<p><span>Foo &amp; bar</span></p>", rendered);
    }
}
