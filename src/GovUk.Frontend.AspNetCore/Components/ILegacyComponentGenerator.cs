namespace GovUk.Frontend.AspNetCore.Components;

/// <summary>
/// Generates GOV.UK Design System components.
/// </summary>
internal interface ILegacyComponentGenerator
{
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
