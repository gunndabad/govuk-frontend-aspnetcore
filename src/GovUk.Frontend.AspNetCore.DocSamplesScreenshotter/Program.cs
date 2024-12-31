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

        var repoRoot = typeof(Program).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "RepoRoot")
            .Value!;

        var docsRoot = Path.Combine(repoRoot, "docs", "images");

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
            ("GovUkFrontendComponent/Accordion/AccordionWithSummarySections", "accordion-with-summary-sections.png"),
            ("GovUkFrontendComponent/BackLink/BackLinkWithDefaultContent", "back-link-with-default-content.png"),
            ("GovUkFrontendComponent/BackLink/BackLinkWithCustomContent", "back-link-with-custom-content.png"),
            ("GovUkFrontendComponent/Breadcrumbs/Breadcrumbs", "breadcrumbs.png"),
            ("GovUkFrontendComponent/Button/DefaultButton", "button-default.png"),
            ("GovUkFrontendComponent/Button/SecondaryButton", "button-secondary.png"),
            ("GovUkFrontendComponent/Button/StartButton", "button-start.png"),
            ("GovUkFrontendComponent/Button/DisabledButton", "button-disabled.png"),
            ("GovUkFrontendComponent/Checkboxes/Checkboxes", "checkboxes.png"),
            ("GovUkFrontendComponent/Checkboxes/CheckboxesWithConditional", "checkboxes-with-conditional.png"),
            ("GovUkFrontendComponent/Checkboxes/CheckboxesWithError", "checkboxes-with-error.png"),
            ("GovUkFrontendComponent/Checkboxes/CheckboxesWithNone", "checkboxes-with-none.png"),
            ("GovUkFrontendComponent/Checkboxes/CheckboxesWithoutFieldset", "checkboxes-without-fieldset.png"),
            ("GovUkFrontendComponent/CharacterCount/CharacterCount", "character-count.png"),
            ("GovUkFrontendComponent/DateInput/DateInput", "date-input.png"),
            ("GovUkFrontendComponent/DateInput/DateInputWithCustomItemLabels", "date-input-with-custom-labels.png"),
            ("GovUkFrontendComponent/DateInput/DateInputWithCustomItemValues", "date-input-with-custom-values.png"),
            ("GovUkFrontendComponent/DateInput/DateInputWithError", "date-input-with-error.png"),
            ("GovUkFrontendComponent/DateInput/DateInputWithFieldset", "date-input-with-fieldset.png"),
            ("GovUkFrontendComponent/Details/Details", "details.png"),
            ("GovUkFrontendComponent/Details/DetailsExpanded", "details-expanded.png"),
            ("GovUkFrontendComponent/ErrorMessage/ErrorMessageWithSpecifiedContent", "error-message-with-specified-content.png"),
            ("GovUkFrontendComponent/ErrorMessage/ErrorMessageWithOverridenVisuallyHiddenText", "error-message-with-overriden-visually-hidden-text.png"),
            ("GovUkFrontendComponent/ErrorMessage/ErrorMessageWithModelStateError", "error-message-with-modelstate-error.png"),
            ("GovUkFrontendComponent/ErrorSummary/ErrorSummary", "error-summary.png"),
            ("GovUkFrontendComponent/ErrorSummary/ErrorSummaryWithModelStateError", "error-summary-with-modelstate-error.png"),
            ("GovUkFrontendComponent/ErrorSummary/ErrorSummaryWithTitle", "error-summary-with-title.png"),
            ("GovUkFrontendComponent/Fieldset/Fieldset", "fieldset.png"),
            ("GovUkFrontendComponent/FileUpload/FileUpload", "file-upload.png"),
            ("GovUkFrontendComponent/FileUpload/FileUploadWithErrors", "file-upload-with-errors.png"),
            ("GovUkFrontendComponent/InsetText/InsetText", "inset-text.png"),
            ("GovUkFrontendComponent/NotificationBanner/NotificationBanner", "notification-banner.png"),
            ("GovUkFrontendComponent/NotificationBanner/NotificationBannerSuccess", "notification-banner-success.png"),
            ("GovUkFrontendComponent/NotificationBanner/NotificationBannerWithOverridenTitle", "notification-banner-with-overriden-title.png"),
            ("GovUkFrontendComponent/Pagination/Pagination", "pagination.png"),
            ("GovUkFrontendComponent/Pagination/Stacked", "pagination-stacked.png"),
            ("GovUkFrontendComponent/Pagination/WithEllipsis", "pagination-with-ellipsis.png"),
            ("GovUkFrontendComponent/Panel/Panel", "panel.png"),
            ("GovUkFrontendComponent/Radios/Radios", "radios.png"),
            ("GovUkFrontendComponent/Radios/RadiosWithConditional", "radios-with-conditional.png"),
            ("GovUkFrontendComponent/Radios/RadiosWithError", "radios-with-error.png"),
            ("GovUkFrontendComponent/Select/Select", "select.png"),
            ("GovUkFrontendComponent/SummaryList/SummaryListWithActions", "summary-list-with-actions.png"),
            ("GovUkFrontendComponent/SummaryList/SummaryListWithCard", "summary-list-with-card.png"),
            ("GovUkFrontendComponent/SummaryList/SummaryListWithoutActions", "summary-list-without-actions.png"),
            ("GovUkFrontendComponent/PhaseBanner/PhaseBanner", "phase-banner.png"),
            ("GovUkFrontendComponent/Tabs/Tabs", "tabs.png"),
            ("GovUkFrontendComponent/Tag/DefaultTag", "tag-default.png"),
            ("GovUkFrontendComponent/Tag/TagWithClass", "tag-with-class.png"),
            ("GovUkFrontendComponent/TextArea/TextArea", "textarea.png"),
            ("GovUkFrontendComponent/TextInput/TextInput", "text-input.png"),
            ("GovUkFrontendComponent/TextInput/TextInputWithError", "text-input-with-error.png"),
            ("GovUkFrontendComponent/TextInput/TextInputWithPrefixAndSuffix", "text-input-with-prefix-and-suffix.png"),
            ("GovUkFrontendComponent/WarningText/WarningText", "warning-text.png")
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
            var screenShotDirectory = Path.GetDirectoryName(fullyQualifiedScreenshotPath)!;
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
