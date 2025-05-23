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
            services.Configure<GovUkFrontendAspNetCoreOptions>(options => options.CompiledContentPath = "");
        });
        await fixture.InitializeAsync();

        var resolvedPath = $"/{fileName.Replace("%version%", GovUkFrontendInfo.Version)}";

        var request = new HttpRequestMessage(HttpMethod.Get, resolvedPath);

        // Act
        var response = await fixture.HttpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
