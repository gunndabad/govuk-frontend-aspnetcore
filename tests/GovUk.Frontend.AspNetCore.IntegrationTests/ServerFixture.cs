using System.Diagnostics;
using Microsoft.Playwright;

namespace GovUk.Frontend.AspNetCore.IntegrationTests;

public class ServerFixture : IAsyncLifetime
{
    public const string BaseUrl = "http://localhost:55342";

    private IHost? _host;
    private IPlaywright? _playright;
    private bool _disposed = false;

    public IBrowser? Browser { get; private set; }

    public IServiceProvider Services => _host!.Services;

    public async virtual Task DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (Browser is not null)
        {
            await Browser.DisposeAsync();
        }

        _playright?.Dispose();

        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
    }

    public async virtual Task InitializeAsync()
    {
        _host = CreateHost();
        await _host.StartAsync();

        _playright = await Playwright.CreateAsync();
        Browser = await _playright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions() { Headless = !Debugger.IsAttached });
    }

    protected virtual void Configure(IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();

        app.UseRouting();
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddGovUkFrontend();
    }

    private IHost CreateHost() => Host.CreateDefaultBuilder(args: Array.Empty<string>())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder
                .UseUrls(BaseUrl)
                .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning))
                .ConfigureServices((context, services) => ConfigureServices(services))
                .Configure(Configure);
        })
        .Build();
}
