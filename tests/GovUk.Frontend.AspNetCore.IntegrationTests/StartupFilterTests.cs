using System.Net;

namespace GovUk.Frontend.AspNetCore.IntegrationTests;

public class StartupFilterTests
{
    [Theory]
    [InlineData("govuk-frontend-%version%.min.css")]
    [InlineData("govuk-frontend-%version%.min.js")]
    public async Task HostsCompiledAssets(string fileName)
    {
        // Arrange
        await using var fixture = new StartupFilterTestFixture(services =>
        {
            services.Configure<GovUkFrontendAspNetCoreOptions>(
                options => options.CompiledContentPath = "/non-standard-compiled");
        });
        await fixture.InitializeAsync();

        var resolvedPath = $"/non-standard-compiled/{fileName.Replace("%version%", GovUkFrontendInfo.Version)}";

        var request = new HttpRequestMessage(HttpMethod.Get, resolvedPath);

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(response.Headers.GetValues("Cache-Control"));
    }

    [Theory]
    [InlineData("images/favicon.ico")]
    [InlineData("images/govuk-crest.svg")]
    [InlineData("fonts/bold-affa96571d-v2.woff")]
    public async Task HostsStaticAssets(string fileName)
    {
        // Arrange
        await using var fixture = new StartupFilterTestFixture(services =>
        {
            services.Configure<GovUkFrontendAspNetCoreOptions>(
                options => options.StaticAssetsContentPath = "/non-standard-asset-location");
        });
        await fixture.InitializeAsync();

        var resolvedPath = $"/non-standard-asset-location/{fileName}";

        var request = new HttpRequestMessage(HttpMethod.Get, resolvedPath);

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RewritesAssetPathInCssFile()
    {
        // Arrange
        await using var fixture = new StartupFilterTestFixture(services =>
        {
            services.Configure<GovUkFrontendAspNetCoreOptions>(options =>
            {
                options.CompiledContentPath = "/non-standard-compiled";
                options.StaticAssetsContentPath = "/non-standard-assets";
            });
        });
        await fixture.InitializeAsync();

        var resolvedPath = $"/non-standard-compiled/govuk-frontend-{GovUkFrontendInfo.Version}.min.css";

        var request = new HttpRequestMessage(HttpMethod.Get, resolvedPath);

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        var css = await response.Content.ReadAsStringAsync();
        Assert.Contains("/non-standard-assets/", css);
        Assert.DoesNotContain("/assets", css);
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

        services.AddRazorPages();

        _configureServices?.Invoke(services);
    }
}
