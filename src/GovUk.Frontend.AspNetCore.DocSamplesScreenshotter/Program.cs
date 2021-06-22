using System;
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
                ("Button/DefaultButton", "button-default.png"),
                ("Button/SecondaryButton", "button-secondary.png"),
                ("Button/StartButton", "button-start.png"),
                ("Button/DisabledButton", "button-disabled.png"),
                ("Details/Details", "details.png"),
                ("Details/DetailsExpanded", "details-expanded.png"),
                ("ErrorMessage/ErrorMessageWithSpecifiedContent", "error-message-with-specified-content.png"),
                ("ErrorMessage/ErrorMessageWithOverridenVisuallyHiddenText", "error-message-with-overriden-visually-hidden-text.png"),
                ("ErrorMessage/ErrorMessageWithModelStateError", "error-message-with-modelstate-error.png"),
                ("InsetText/InsetText", "inset-text.png"),
                ("NotificationBanner/NotificationBanner", "notification-banner.png"),
                ("NotificationBanner/NotificationBannerSuccess", "notification-banner-success.png"),
                ("NotificationBanner/NotificationBannerWithOverridenTitle", "notification-banner-with-overriden-title.png"),
                ("Panel/Panel", "panel.png"),
                ("SummaryList/SummaryListWithActions", "summary-list-with-actions.png"),
                ("SummaryList/SummaryListWithoutActions", "summary-list-without-actions.png"),
                ("PhaseBanner/PhaseBanner", "phase-banner.png"),
                ("Tag/DefaultTag", "tag-default.png"),
                ("Tag/TagWithClass", "tag-with-class.png"),
                ("WarningText/WarningText", "warning-text.png")
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
                var response = await page.GoToAsync(fullyQualifiedPagePath);

                if (!response.Ok)
                {
                    throw new ArgumentException(
                        $"Unsuccessful response ({(int)response.Status}) for '{pagePath}'.",
                        nameof(pagePath));
                }

                var container = await page.WaitForSelectorAsync("#container");
                await container.ScreenshotAsync(fullyQualifiedScreenshotPath);
            }
        }
    }
}
