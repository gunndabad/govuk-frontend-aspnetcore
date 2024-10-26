using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.ComponentGeneration;

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
            .AddAttribute("class", "govuk-link", encodeValue: false)
            .AddAttribute("href", "page?foo=bar&baz=qux", encodeValue: false);

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
            .AddBooleanAttribute("disabled");

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
            .AppendText("Foo & bar");

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
            .AppendHtml("<span>Foo &amp; bar</span>");

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
            .AppendHtml(new HtmlString("<span>Foo &amp; bar</span>"));

        // Act
        var rendered = builder.ToString();

        // Assert
        Assert.Equal("<p><span>Foo &amp; bar</span></p>", rendered);
    }
}
