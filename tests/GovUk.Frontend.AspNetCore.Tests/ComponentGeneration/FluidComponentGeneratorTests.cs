using System;
using System.Text.RegularExpressions;
using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.ComponentGeneration;

public class FluidComponentGeneratorTests
{
    private static readonly Regex _decimalEncodedHtmlPattern = new("&#(\\d+);");

    private readonly FluidComponentGenerator _componentGenerator = new();

    [Theory]
    [ComponentFixtureData("accordion", typeof(AccordionOptions2), exclude: ["with falsy values"])]
    public void Accordion(ComponentTestCaseData<AccordionOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateAccordion(options),
            amendExpectedHtml: data.Name is "default" or "with translations" ? html => html.Replace("’", "&#x2019;") : null);

    [Theory]
    [ComponentFixtureData("back-link", typeof(BackLinkOptions))]
    public void BackLink(ComponentTestCaseData<BackLinkOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateBackLink(options));

    [Theory]
    [ComponentFixtureData("breadcrumbs", typeof(BreadcrumbsOptions2))]
    public void Breadcrumbs(ComponentTestCaseData<BreadcrumbsOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateBreadcrumbs(options));

    [Theory]
    [ComponentFixtureData("button", typeof(ButtonOptions2))]
    public void Button(ComponentTestCaseData<ButtonOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateButton(options));

    [Theory]
    [ComponentFixtureData("checkboxes", typeof(CheckboxesOptions2), exclude: ["with falsy values"])]
    public void Checkboxes(ComponentTestCaseData<CheckboxesOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateCheckboxes(options));

    [Theory]
    [ComponentFixtureData("cookie-banner", typeof(CookieBannerOptions))]
    public void CookieBanner(ComponentTestCaseData<CookieBannerOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateCookieBanner(options),
            includeCharacterByCharacterComparison: false);

    [Theory]
    [ComponentFixtureData("error-message", typeof(ErrorMessageOptions2))]
    public void ErrorMessage(ComponentTestCaseData<ErrorMessageOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateErrorMessage(options));

    [Theory]
    [ComponentFixtureData("fieldset", typeof(FieldsetOptions2))]
    public void Fieldset(ComponentTestCaseData<FieldsetOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateFieldset(options));

    [Theory]
    [ComponentFixtureData("file-upload", typeof(FileUploadOptions), only: "translated")]
    public void FileUpload(ComponentTestCaseData<FileUploadOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateFileUpload(options),
            amendExpectedHtml: html => html.Replace("C:&#x5C;fakepath&#x5C;myphoto.jpg", "C:\\fakepath\\myphoto.jpg"));

    [Theory]
    [ComponentFixtureData("hint", typeof(HintOptions2))]
    public void Hint(ComponentTestCaseData<HintOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateHint(options),
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
    public void Label(ComponentTestCaseData<LabelOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateLabel(options));

    [Theory]
    [ComponentFixtureData("warning-text", typeof(WarningTextOptions))]
    public void WarningText(ComponentTestCaseData<WarningTextOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateWarningText(options),
            amendExpectedHtml: html => html.Replace("£", "&#xA3;").Replace("’", "&#x2019;"));

    private void CheckComponentHtmlMatchesExpectedHtml<TOptions>(
        ComponentTestCaseData<TOptions> testCaseData,
        Func<FluidComponentGenerator, TOptions, IHtmlContent> generateComponent,
        bool includeCharacterByCharacterComparison = true,
        Predicate<IDiff>? excludeDiff = null,
        Func<string, string>? amendExpectedHtml = null)
    {
        var html = generateComponent(_componentGenerator, testCaseData.Options).ToHtmlString();

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
        AssertEx.HtmlEqual(expectedHtml, html, excludeDiff);

        // For exact character-by-character equality
        if (includeCharacterByCharacterComparison)
        {
            Assert.Equal(expectedHtml, html);
        }
    }
}
