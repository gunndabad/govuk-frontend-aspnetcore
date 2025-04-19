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
    [ComponentFixtureData("accordion", typeof(AccordionOptions))]
    public void Accordion(ComponentTestCaseData<AccordionOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateAccordion(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("back-link", typeof(BackLinkOptions))]
    public void BackLink(ComponentTestCaseData<BackLinkOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateBackLink(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("breadcrumbs", typeof(BreadcrumbsOptions))]
    public void Breadcrumbs(ComponentTestCaseData<BreadcrumbsOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateBreadcrumbs(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("button", typeof(ButtonOptions))]
    public void Button(ComponentTestCaseData<ButtonOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateButton(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("character-count", typeof(CharacterCountOptions))]
    public void CharacterCount(ComponentTestCaseData<AccordionOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateCharacterCount(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("checkboxes", typeof(CheckboxesOptions))]
    public void Checkboxes(ComponentTestCaseData<CheckboxesOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateCheckboxes(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("cookie-banner", typeof(CookieBannerOptions))]
    public void CookieBanner(ComponentTestCaseData<CookieBannerOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateCookieBanner(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("date-input", typeof(DateInputOptions))]
    public void DateInput(ComponentTestCaseData<CookieBannerOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateDateInput(options).ToHtmlString());

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
    [ComponentFixtureData("exit-this-page", typeof(ExitThisPageOptions))]
    public void ExitThisPage(ComponentTestCaseData<ExitThisPageOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateExitThisPage(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("fieldset", typeof(FieldsetOptions))]
    public void Fieldset(ComponentTestCaseData<FieldsetOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateFieldset(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("file-upload", typeof(FileUploadOptions))]
    public void FileUpload(ComponentTestCaseData<FileUploadOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateFileUpload(options).ToHtmlString());

    // [Theory]
    // [ComponentFixtureData("footer", typeof(FooterOptions))]
    // public void Footer(ComponentTestCaseData<FooterOptions> data) =>
    //     CheckComponentHtmlMatchesExpectedHtml(
    //         data,
    //         (generator, options) => generator.GenerateFooter(options).ToHtmlString());

    // [Theory]
    // [ComponentFixtureData("header", typeof(HeaderOptions))]
    // public void Header(ComponentTestCaseData<HeaderOptions> data) =>
    //     CheckComponentHtmlMatchesExpectedHtml(
    //         data,
    //         (generator, options) => generator.GenerateHeader(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("hint", typeof(HintOptions))]
    public void Hint(ComponentTestCaseData<HintOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateHint(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("inset-text", typeof(TextInputOptions))]
    public void InsetText(ComponentTestCaseData<InsetTextOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateInsetText(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("label", typeof(LabelOptions))]
    public void Label(ComponentTestCaseData<LabelOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateLabel(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("notification-banner", typeof(NotificationBannerOptions))]
    public void NotificationBanner(ComponentTestCaseData<NotificationBannerOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateNotificationBanner(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("pagination", typeof(PaginationOptions))]
    public void Pagination(ComponentTestCaseData<PaginationOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GeneratePagination(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("panel", typeof(PaginationOptions))]
    public void Panel(ComponentTestCaseData<PanelOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GeneratePanel(options).ToHtmlString());

    // [Theory]
    // [ComponentFixtureData("password-input", typeof(PasswordInputOptions))]
    // public void PasswordInput(ComponentTestCaseData<PasswordInputOptions> data) =>
    //     CheckComponentHtmlMatchesExpectedHtml(
    //         data,
    //         (generator, options) => generator.GeneratePasswordInput(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("phase-banner", typeof(PhaseBannerOptions))]
    public void PhaseBanner(ComponentTestCaseData<PhaseBannerOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GeneratePhaseBanner(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("radios", typeof(RadiosOptions))]
    public void Radios(ComponentTestCaseData<RadiosOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateRadios(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("select", typeof(SelectOptions))]
    public void Select(ComponentTestCaseData<SelectOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateSelect(options).ToHtmlString());

    // [Theory]
    // [ComponentFixtureData("service-navigation", typeof(ServiceNavigationOptions))]
    // public void ServiceNavigation(ComponentTestCaseData<ServiceNavigationOptions> data) =>
    //     CheckComponentHtmlMatchesExpectedHtml(
    //         data,
    //         (generator, options) => generator.GenerateServiceNavigation(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("skip-link", typeof(SkipLinkOptions))]
    public void SkipLink(ComponentTestCaseData<SkipLinkOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateSkipLink(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("summary-list", typeof(SummaryListOptions))]
    public void SummaryList(ComponentTestCaseData<SummaryListOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateSummaryList(options).ToHtmlString());

    // [Theory]
    // [ComponentFixtureData("table", typeof(TableOptions))]
    // public void Table(ComponentTestCaseData<TableOptions> data) =>
    //     CheckComponentHtmlMatchesExpectedHtml(
    //         data,
    //         (generator, options) => generator.GenerateTable(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("tabs", typeof(TabsOptions))]
    public void Tabs(ComponentTestCaseData<TabsOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTabs(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("tag", typeof(TagOptions))]
    public void Tag(ComponentTestCaseData<TagOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTag(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("task-list", typeof(TaskListOptions))]
    public void TaskList(ComponentTestCaseData<TaskListOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTaskList(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("textarea", typeof(TextareaOptions))]
    public void Textarea(ComponentTestCaseData<TextareaOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTextarea(options).ToHtmlString());

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
