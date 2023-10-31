using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GovUk.Frontend.AspNetCore.IntegrationTests;

public class StartupFilterTests
{
    [Theory]
    [InlineData("all.min.css")]
    [InlineData("all.min.js")]
    public async Task HostsCompiledAssets(string fileName)
    {
        // Arrange
        await using var fixture = new StartupFilterTestFixture(services =>
        {
            services.Configure<GovUkFrontendAspNetCoreOptions>(options => options.CompiledContentPath = "/govuk");
        });
        await fixture.InitializeAsync();

        var resolvedPath = $"/govuk/{fileName}";

        var request = new HttpRequestMessage(HttpMethod.Get, resolvedPath);

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("images/favicon.ico")]
    [InlineData("images/govuk-crest.png")]
    [InlineData("fonts/bold-affa96571d-v2.woff")]
    public async Task HostsStaticAssets(string fileName)
    {
        // Arrange
        await using var fixture = new StartupFilterTestFixture(services =>
        {
            services.Configure<GovUkFrontendAspNetCoreOptions>(options => options.StaticAssetsContentPath = "/assets");
        });
        await fixture.InitializeAsync();

        var resolvedPath = $"/assets/{fileName}";

        var request = new HttpRequestMessage(HttpMethod.Get, resolvedPath);

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddImportsToHtmlIsFalse_DoesNotAddStyleAndScriptImportsToRazorViews()
    {
        // Arrange
        await using var fixture = new StartupFilterTestFixture(services =>
        {
            services.Configure<GovUkFrontendAspNetCoreOptions>(options => options.AddImportsToHtml = false);
        });
        await fixture.InitializeAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, "/Empty");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var html = await response.GetHtmlDocument();

        var head = html.QuerySelector("head");

        var linkTags = head.QuerySelectorAll("link");

        Assert.DoesNotContain(
            $"/govuk-frontend-{Constants.GdsLibraryVersion}.min.css",
            linkTags
                .Where(t => t.GetAttribute("rel") == "stylesheet")
                .Select(t => t.GetAttribute("href")));

        var body = html.QuerySelector("body");

        Assert.DoesNotContain("govuk-template__body", body.ClassList);

        var bodyScriptTags = body.QuerySelectorAll("script");

        Assert.DoesNotContain(
            $"/govuk-frontend-{Constants.GdsLibraryVersion}.min.js",
            bodyScriptTags.Select(t => t.GetAttribute("src")));

        Assert.DoesNotContain(
            $"window.GOVUKFrontend.initAll()",
            bodyScriptTags.Select(t => t.InnerHtml));
    }

    [Fact]
    public async Task AddImportsToHtmlIsTrue_AddsStyleAndScriptImportsToRazorViews()
    {
        // Arrange
        await using var fixture = new StartupFilterTestFixture(services =>
        {
            services.Configure<GovUkFrontendAspNetCoreOptions>(options => options.AddImportsToHtml = true);
        });
        await fixture.InitializeAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, "/Empty");

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();

        var html = await response.GetHtmlDocument();

        var head = html.QuerySelector("head");

        var linkTags = head.QuerySelectorAll("link");

        Assert.Contains(
            $"/govuk-frontend-{Constants.GdsLibraryVersion}.min.css",
            linkTags
                .Where(t => t.GetAttribute("rel") == "stylesheet")
                .Select(t => t.GetAttribute("href")));

        var body = html.QuerySelector("body");

        Assert.Contains("govuk-template__body", body.ClassList);

        var bodyScriptTags = body.QuerySelectorAll("script");

        Assert.Contains(
            $"/govuk-frontend-{Constants.GdsLibraryVersion}.min.js",
            bodyScriptTags.Select(t => t.GetAttribute("src")));

        Assert.Contains(
            $"window.GOVUKFrontend.initAll()",
            bodyScriptTags.Select(t => t.InnerHtml));
    }
}

public class StartupFilterTestFixture : ServerFixture
{
    private readonly Action<IServiceCollection> _configureServices;

    public StartupFilterTestFixture(Action<IServiceCollection> configureServices)
    {
        _configureServices = configureServices;

        HttpClient = new HttpClient()
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }

    public HttpClient HttpClient { get; }

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

        services.AddRazorPages();

        _configureServices?.Invoke(services);
    }
}
