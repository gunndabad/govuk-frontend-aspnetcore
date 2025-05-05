using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.Components;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.Components;

public class DefaultComponentGeneratorTests
{
    private static readonly Regex _decimalEncodedHtmlPattern = new("&#(\\d+);");

    private readonly DefaultComponentGenerator _componentGenerator = new();

    [Theory]
    [ComponentFixtureData("accordion", typeof(AccordionOptions2), exclude: ["with falsy values"])]
    public Task Accordion(ComponentTestCaseData<AccordionOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateAccordionAsync(options),
            amendExpectedHtml: data.Name is "default" or "with translations" ? html => html.Replace("’", "&#x2019;") : null);

    [Theory]
    [ComponentFixtureData("back-link", typeof(BackLinkOptions))]
    public Task BackLink(ComponentTestCaseData<BackLinkOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateBackLinkAsync(options));

    [Theory]
    [ComponentFixtureData("breadcrumbs", typeof(BreadcrumbsOptions))]
    public Task Breadcrumbs(ComponentTestCaseData<BreadcrumbsOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateBreadcrumbsAsync(options));

    [Theory]
    [ComponentFixtureData("button", typeof(ButtonOptions))]
    public Task Button(ComponentTestCaseData<ButtonOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateButtonAsync(options));

    [Theory]
    [ComponentFixtureData("checkboxes", typeof(CheckboxesOptions2), exclude: ["with falsy values"])]
    public Task Checkboxes(ComponentTestCaseData<CheckboxesOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateCheckboxesAsync(options));

    [Theory]
    [ComponentFixtureData("cookie-banner", typeof(CookieBannerOptions))]
    public Task CookieBanner(ComponentTestCaseData<CookieBannerOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateCookieBannerAsync(options),
            amendExpectedHtml: html => Regex.Replace(html, "</div>\n\n\n  </div>\n</div>$", "</div>\n\n  </div>\n</div>"));

    [Theory]
    [ComponentFixtureData("error-message", typeof(ErrorMessageOptions2))]
    public Task ErrorMessage(ComponentTestCaseData<ErrorMessageOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateErrorMessageAsync(options));

    [Theory]
    [ComponentFixtureData("error-summary", typeof(ErrorSummaryOptions))]
    public Task ErrorSummary(ComponentTestCaseData<ErrorSummaryOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateErrorSummaryAsync(options));

    [Theory]
    [ComponentFixtureData("fieldset", typeof(FieldsetOptions2))]
    public Task Fieldset(ComponentTestCaseData<FieldsetOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateFieldsetAsync(options));

    [Theory]
    [ComponentFixtureData("file-upload", typeof(FileUploadOptions), only: "translated")]
    public Task FileUpload(ComponentTestCaseData<FileUploadOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateFileUploadAsync(options),
            amendExpectedHtml: html => html.Replace("C:&#x5C;fakepath&#x5C;myphoto.jpg", "C:\\fakepath\\myphoto.jpg"));

    [Theory]
    [ComponentFixtureData("hint", typeof(HintOptions2))]
    public Task Hint(ComponentTestCaseData<HintOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateHintAsync(options),
            amendExpectedHtml:
                // Account for weird encoding differences
                data.Name == "default" ?
                    html => html.Replace(
                    "\nFor example, &#x27;QQ 12 34 56 C&#x27;.\n",
                    "&#xA;For example, &#x27;QQ 12 34 56 C&#x27;.&#xA;",
                    StringComparison.OrdinalIgnoreCase) :
                null);

    [Theory]
    [ComponentFixtureData("label", typeof(LabelOptions2))]
    public Task Label(ComponentTestCaseData<LabelOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateLabelAsync(options));

    [Theory]
    [ComponentFixtureData("service-navigation", typeof(ServiceNavigationOptions))]
    public Task ServiceNavigation(ComponentTestCaseData<ServiceNavigationOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateServiceNavigationAsync(options),
            compareWhitespace: false);

    [Theory]
    [ComponentFixtureData("tag", typeof(TagOptions))]
    public Task Tag(ComponentTestCaseData<TagOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTagAsync(options));

    [Theory]
    [ComponentFixtureData("warning-text", typeof(WarningTextOptions))]
    public Task WarningText(ComponentTestCaseData<WarningTextOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateWarningTextAsync(options),
            amendExpectedHtml: html => html.Replace("£", "&#xA3;").Replace("’", "&#x2019;"));

    private async Task CheckComponentHtmlMatchesExpectedHtml<TOptions>(
        ComponentTestCaseData<TOptions> testCaseData,
        Func<DefaultComponentGenerator, TOptions, ValueTask<IHtmlContent>> generateComponent,
        bool compareWhitespace = true,
        Predicate<IDiff>? excludeDiff = null,
        Func<string, string>? amendExpectedHtml = null)
    {
        var html = (await generateComponent(_componentGenerator, testCaseData.Options)).ToHtmlString();

        // Some of the fixtures have characters with different encodings to what we produce;
        // normalize those before comparing:
        var expectedHtml = _decimalEncodedHtmlPattern
            .Replace(
                testCaseData.ExpectedHtml,
                (Match match) =>
                {
                    var encodedDecimal = int.Parse(match.Groups[1].Value);
                    return $"&#x{encodedDecimal:X};";
                });

        if (amendExpectedHtml is not null)
        {
            expectedHtml = amendExpectedHtml(expectedHtml);
        }

        // Semantic comparison
        //AssertEx.HtmlEqual(expectedHtml, html, excludeDiff);

        // For exact character-by-character equality
        if (compareWhitespace)
        {
            Assert.Equal(expectedHtml, html);
        }
    }
}
