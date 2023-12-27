using System;
using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.ComponentGeneration;

public class ComponentTests
{
    private readonly DefaultComponentGenerator _componentGenerator;

    public ComponentTests()
    {
        _componentGenerator = new DefaultComponentGenerator();
    }

    [Theory]
    [ComponentFixtureData("back-link", typeof(BackLinkOptions))]
    public void BackLink(ComponentTestCaseData<BackLinkOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateBackLink(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("button", typeof(ButtonOptions))]
    public void Button(ComponentTestCaseData<ButtonOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateButton(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("details", typeof(DetailsOptions))]
    public void Details(ComponentTestCaseData<DetailsOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateDetails(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("warning-text", typeof(WarningTextOptions))]
    public void WarningText(ComponentTestCaseData<WarningTextOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateWarningText(options).ToHtmlString());

    private void CheckComponentHtmlMatchesExpectedHtml<TOptions>(
        ComponentTestCaseData<TOptions> testCaseData,
        Func<IComponentGenerator, TOptions, string> generateComponent,
        Predicate<IDiff>? excludeDiff = null)
    {
        var html = generateComponent(_componentGenerator, testCaseData.Options);

        AssertEx.HtmlEqual(testCaseData.ExpectedHtml, html, excludeDiff);
    }
}
