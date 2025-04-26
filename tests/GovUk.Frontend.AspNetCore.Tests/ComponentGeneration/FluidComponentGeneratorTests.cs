using System;
using AngleSharp.Diffing.Core;
using Fluid.Values;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.ComponentGeneration;

public class FluidComponentGeneratorTests
{
    private readonly FluidComponentGenerator _componentGenerator = new();

    [Theory]
    [ComponentFixtureData("label", typeof(LabelOptions2))]
    public void Label(ComponentTestCaseData<LabelOptions2> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateLabel(options));

    private void CheckComponentHtmlMatchesExpectedHtml<TOptions>(
        ComponentTestCaseData<TOptions> testCaseData,
        Func<FluidComponentGenerator, TOptions, string> generateComponent,
        Predicate<IDiff>? excludeDiff = null)
    {
        var html = generateComponent(_componentGenerator, testCaseData.Options);

        // Some of the fixtures have characters with different encodings to what we produce;
        // normalize those before comparing:
        var expectedHtml = testCaseData.ExpectedHtml
            .Replace("’", "&#x2019;")
            .Replace("&#39;", "&#x27;")
            .Replace("\u00a3", "&#xA3;");

        // Semantic comparison
        AssertEx.HtmlEqual(expectedHtml, html, excludeDiff);

        // For exact character-by-character equality
        Assert.Equal(expectedHtml, html);
    }
}
