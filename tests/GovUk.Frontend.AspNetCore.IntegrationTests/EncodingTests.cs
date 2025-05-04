using System;
using System.Net.Http;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GovUk.Frontend.AspNetCore.IntegrationTests;

public class EncodingTests(EncodingsTestFixture fixture) : IClassFixture<EncodingsTestFixture>
{
    [Fact]
    public async Task ExplicitHrefIsEncoded()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/Encodings");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        var doc = await AssertEx.GetHtmlDocument(response);
        Assert.Equal(
            "foo?bar=baz&amp;qux=foo",
            doc.DocumentElement.GetElementByTestId("String")?.GetAttribute("href"));
    }

    [Fact]
    public async Task HrefFromStringVariableIsEncoded()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/Encodings");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        var doc = await AssertEx.GetHtmlDocument(response);
        Assert.Equal(
            "foo?bar=baz&amp;qux=foo",
            doc.DocumentElement.GetElementByTestId("Variable")?.GetAttribute("href"));
    }

    [Fact]
    public async Task ExplicitDataAttributeIsEncoded()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/Encodings");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        var doc = await AssertEx.GetHtmlDocument(response);
        Assert.Equal(
            "Foo &amp; Bar",
            doc.DocumentElement.GetElementByTestId("String")?.GetAttribute("data-foo"));
    }

    [Fact]
    public async Task DataAttributeFromStringVariableIsEncoded()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/Encodings");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        var doc = await AssertEx.GetHtmlDocument(response);
        Assert.Equal(
            "Foo &amp; Bar",
            doc.DocumentElement.GetElementByTestId("Variable")?.GetAttribute("data-foo"));
    }
}

public class EncodingsTestFixture : ServerFixture
{
    public EncodingsTestFixture()
    {
        HttpClient = new HttpClient()
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }

    public HttpClient HttpClient { get; }

    public override Task DisposeAsync()
    {
        HttpClient.Dispose();
        return base.DisposeAsync();
    }

    protected override void Configure(IApplicationBuilder app)
    {
        base.Configure(app);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services
            .AddRazorPages();
    }
}
