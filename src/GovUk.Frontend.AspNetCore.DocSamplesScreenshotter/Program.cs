using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.DocSamples;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace GovUk.Frontend.AspNetCore.DocSamplesScreenshotter;

class Program
{
    static async Task Main()
    {
        const string baseUrl = "http://localhost:9919";

        var repoRoot = typeof(Program)
            .Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "RepoRoot")
            .Value!;

        var docsRoot = Path.Combine(repoRoot, "docs", "images");

        var hostBuilder = CreateHostBuilder();
        using var host = hostBuilder.Build();
        await host.StartAsync();

        var logger = host
            .Services.GetRequiredService<ILoggerFactory>()
            .CreateLogger("GovUk.Frontend.AspNetCore.DocSamplesScreenshotter");

        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions() { Headless = true });

        await WriteScreenshots(
            new[]
            {
                ("Accordion/AccordionWithSummarySections", "accordion-with-summary-sections.png"),
                ("BackLink/BackLinkWithDefaultContent", "back-link-with-default-content.png"),
                ("BackLink/BackLinkWithCustomContent", "back-link-with-custom-content.png"),
                ("Breadcrumbs/Breadcrumbs", "breadcrumbs.png"),
                ("Button/DefaultButton", "button-default.png"),
                ("Button/SecondaryButton", "button-secondary.png"),
                ("Button/StartButton", "button-start.png"),
                ("Button/DisabledButton", "button-disabled.png"),
                ("Checkboxes/Checkboxes", "checkboxes.png"),
                ("Checkboxes/CheckboxesWithConditional", "checkboxes-with-conditional.png"),
                ("Checkboxes/CheckboxesWithError", "checkboxes-with-error.png"),
                ("Checkboxes/CheckboxesWithNone", "checkboxes-with-none.png"),
                ("Checkboxes/CheckboxesWithoutFieldset", "checkboxes-without-fieldset.png"),
                ("CharacterCount/CharacterCount", "character-count.png"),
                ("DateInput/DateInput", "date-input.png"),
                ("DateInput/DateInputWithCustomItemLabels", "date-input-with-custom-labels.png"),
                ("DateInput/DateInputWithCustomItemValues", "date-input-with-custom-values.png"),
                ("DateInput/DateInputWithError", "date-input-with-error.png"),
                ("DateInput/DateInputWithFieldset", "date-input-with-fieldset.png"),
                ("Details/Details", "details.png"),
                ("Details/DetailsExpanded", "details-expanded.png"),
                ("ErrorSummary/ErrorSummary", "error-summary.png"),
                ("ErrorSummary/ErrorSummaryWithModelStateError", "error-summary-with-modelstate-error.png"),
                ("ErrorSummary/ErrorSummaryWithTitle", "error-summary-with-title.png"),
                ("FileUpload/FileUpload", "file-upload.png"),
                ("FileUpload/FileUploadWithErrors", "file-upload-with-errors.png"),
                ("InsetText/InsetText", "inset-text.png"),
                ("NotificationBanner/NotificationBanner", "notification-banner.png"),
                ("NotificationBanner/NotificationBannerSuccess", "notification-banner-success.png"),
                (
                    "NotificationBanner/NotificationBannerWithOverridenTitle",
                    "notification-banner-with-overriden-title.png"
                ),
                ("Pagination/Pagination", "pagination.png"),
                ("Pagination/Stacked", "pagination-stacked.png"),
                ("Pagination/WithEllipsis", "pagination-with-ellipsis.png"),
                ("Panel/Panel", "panel.png"),
                ("Radios/Radios", "radios.png"),
                ("Radios/RadiosWithConditional", "radios-with-conditional.png"),
                ("Radios/RadiosWithError", "radios-with-error.png"),
                ("Select/Select", "select.png"),
                ("SummaryList/SummaryListWithActions", "summary-list-with-actions.png"),
                ("SummaryList/SummaryListWithCard", "summary-list-with-card.png"),
                ("SummaryList/SummaryListWithoutActions", "summary-list-without-actions.png"),
                ("PhaseBanner/PhaseBanner", "phase-banner.png"),
                ("Tabs/Tabs", "tabs.png"),
                ("Tag/DefaultTag", "tag-default.png"),
                ("Tag/TagWithClass", "tag-with-class.png"),
                ("TextArea/TextArea", "textarea.png"),
                ("TextInput/TextInput", "text-input.png"),
                ("TextInput/TextInputWithError", "text-input-with-error.png"),
                ("TextInput/TextInputWithPrefixAndSuffix", "text-input-with-prefix-and-suffix.png"),
                ("TextInput/TextInputWithModelBinding", "text-input-with-modelbinding.png"),
                ("WarningText/WarningText", "warning-text.png"),
            }
        );

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
            var screenShotDirectory = Path.GetDirectoryName(fullyQualifiedScreenshotPath)!;
            Directory.CreateDirectory(screenShotDirectory);

            await using var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(fullyQualifiedPagePath);

            if (!response.Ok)
            {
                throw new ArgumentException(
                    $"Unsuccessful response ({(int)response.Status}) for '{pagePath}'.",
                    nameof(pagePath)
                );
            }

            var container = await page.WaitForSelectorAsync("#container");
            await container.ScreenshotAsync(fullyQualifiedScreenshotPath);
        }
    }
}
