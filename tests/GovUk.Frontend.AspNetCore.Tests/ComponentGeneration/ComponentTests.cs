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
    [ComponentFixtureData("button", typeof(ButtonOptions))]
    public void Button(ComponentTestCaseData<ButtonOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateButton(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("character-count", typeof(CharacterCountOptions))]
    public void CharacterCount(ComponentTestCaseData<CharacterCountOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateCharacterCount(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("exit-this-page", typeof(ExitThisPageOptions), exclude: ["testing"])]
    public void ExitThisPage(ComponentTestCaseData<ExitThisPageOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateExitThisPage(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("tag", typeof(TagOptions))]
    public void Tag(ComponentTestCaseData<TagOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTag(options).ToHtmlString());

    [Theory]
    [ComponentFixtureData("task-list", typeof(TaskListOptions), exclude: ["with empty values"])]
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
    [ComponentFixtureData("input", typeof(TextInputOptions), exclude: ["with extra letter spacing", "disabled"])]
    public void TextInput(ComponentTestCaseData<TextInputOptions> data) =>
        CheckComponentHtmlMatchesExpectedHtml(
            data,
            (generator, options) => generator.GenerateTextInput(options).ToHtmlString());

    private void CheckComponentHtmlMatchesExpectedHtml<TOptions>(
        ComponentTestCaseData<TOptions> testCaseData,
        Func<ILegacyComponentGenerator, TOptions, string> generateComponent,
        Predicate<IDiff>? excludeDiff = null)
    {
        var html = generateComponent(_componentGenerator, testCaseData.Options);

        AssertEx.HtmlEqual(testCaseData.ExpectedHtml, html, excludeDiff);
    }
}
