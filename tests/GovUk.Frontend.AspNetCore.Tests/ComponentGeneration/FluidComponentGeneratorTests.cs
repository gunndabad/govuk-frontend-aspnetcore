using System;
using System.Text.RegularExpressions;
using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
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
    [ComponentFixtureData("error-message", typeof(ErrorMessageOptions2))]
    public void ErrorMessage(ComponentTestCaseData<ErrorMessageOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateErrorMessage(options));

    [Theory]
    [ComponentFixtureData("file-upload", typeof(FileUploadOptions2), exclude: ["with value"])]
    public void FileUpload(ComponentTestCaseData<FileUploadOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateFileUpload(options));

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

    private void CheckComponentHtmlMatchesExpectedHtml<TOptions>(
        ComponentTestCaseData<TOptions> testCaseData,
        Func<FluidComponentGenerator, TOptions, string> generateComponent,
        Predicate<IDiff>? excludeDiff = null,
        Func<string, string>? amendExpectedHtml = null)
    {
        var html = generateComponent(_componentGenerator, testCaseData.Options);

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
        Assert.Equal(expectedHtml, html);
    }
}
