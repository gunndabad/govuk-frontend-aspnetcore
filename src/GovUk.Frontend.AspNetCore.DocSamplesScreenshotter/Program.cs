using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.DocSamples;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace GovUk.Frontend.AspNetCore.DocSamplesScreenshotter
{
    class Program
    {
        static async Task Main()
        {
            const string baseUrl = "http://localhost:9919";
            const string docsRoot = "../../../../../docs/images/";

            var hostBuilder = CreateHostBuilder();
            using var host = hostBuilder.Build();
            await host.StartAsync();

            var logger = host.Services.GetRequiredService<ILoggerFactory>()
                .CreateLogger("GovUk.Frontend.AspNetCore.DocSamplesScreenshotter");

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions()
            {
                Headless = true
            });

            await WriteScreenshots(new[]
            {
                ("Accordion/AccordionWithSummarySections", "accordion-with-summary-sections.png"),
                ("BackLink/BackLinkWithDefaultContent", "back-link-with-default-content.png"),
                ("BackLink/BackLinkWithCustomContent", "back-link-with-custom-content.png"),
                ("Breadcrumbs/Breadcrumbs", "breadcrumbs.png"),
            });

            await browser.CloseAsync();
            await host.StopAsync();

            static IHostBuilder CreateHostBuilder() =>
                Host.CreateDefaultBuilder()
                    .ConfigureLogging(logging =>
                    {
                        logging
                            .AddFilter("System", LogLevel.Warning)
                            .AddFilter("Microsoft", LogLevel.Warning)
                            .AddFilter("GovUk.Frontend.AspNetCore", LogLevel.Information);
                    })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseUrls(baseUrl);
                    });

            async Task WriteScreenshots(IEnumerable<(string PathPath, string ScreenshotPath)> paths)
            {
                foreach (var (pagePath, screenshotPath) in paths)
                {
                    await WriteScreenshot(pagePath, screenshotPath);
                }
            }

            async Task WriteScreenshot(string pagePath, string screenshotPath)
            {
                logger.LogInformation($"{pagePath} -> {screenshotPath}");

                var fullyQualifiedPagePath = $"{baseUrl}/{pagePath}";

                var fullyQualifiedScreenshotPath = Path.GetFullPath($"{docsRoot}/{screenshotPath}");
                var screenShotDirectory = Path.GetDirectoryName(fullyQualifiedScreenshotPath);
                Directory.CreateDirectory(screenShotDirectory);

                await using var page = await browser.NewPageAsync();
                await page.GoToAsync(fullyQualifiedPagePath);

                var container = await page.WaitForSelectorAsync("#container");
                await container.ScreenshotAsync(fullyQualifiedScreenshotPath);
            }
        }
    }
}
