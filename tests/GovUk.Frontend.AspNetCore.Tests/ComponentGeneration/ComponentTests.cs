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
    [ComponentFixtureData("error-message", typeof(ErrorMessageOptions))]
    public void ErrorMessage(ComponentTestCaseData<ErrorMessageOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateErrorMessage(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("error-summary", typeof(ErrorSummaryOptions))]
    public void ErrorSummary(ComponentTestCaseData<ErrorSummaryOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateErrorSummary(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("fieldset", typeof(FieldsetOptions))]
    public void Fieldset(ComponentTestCaseData<FieldsetOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateFieldset(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("hint", typeof(HintOptions))]
    public void Hint(ComponentTestCaseData<HintOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateHint(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("label", typeof(LabelOptions), exclude: "empty")]
    public void Label(ComponentTestCaseData<LabelOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateLabel(options).ToHtmlString());

    // Fixture data for Phase banner is poor - most of the variations don't specify 'tag', which isn't valid
    [Theory]
    [ComponentFixtureData("phase-banner", typeof(PhaseBannerOptions), exclude: new[] { "classes", "html", "html as text", "attributes", "text" })]
    public void PhaseBanner(ComponentTestCaseData<PhaseBannerOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GeneratePhaseBanner(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("tag", typeof(TagOptions))]
    public void Tag(ComponentTestCaseData<TagOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTag(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("input", typeof(TextInputOptions))]
    public void TextInput(ComponentTestCaseData<TextInputOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTextInput(options).ToHtmlString());

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
