namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// Generates GOV.UK Design System components.
/// </summary>
public interface IComponentGenerator
{
    /// <summary>
    /// Generates a breadcrumbs component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateBreadcrumbs(BreadcrumbsOptions options);

    /// <summary>
    /// Generates a button component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateButton(ButtonOptions options);

    /// <summary>
    /// Generates a character count component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateCharacterCount(CharacterCountOptions options);

    /// <summary>
    /// Generates an exit this page component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateExitThisPage(ExitThisPageOptions options);

    /// <summary>
    /// Generates a tag component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateTag(TagOptions options);

    /// <summary>
    /// Generates a task list component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateTaskList(TaskListOptions options);

    /// <summary>
    /// Generates a textarea component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateTextarea(TextareaOptions options);

    /// <summary>
    /// Generates a text input component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateTextInput(TextInputOptions options);
}
