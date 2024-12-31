namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// Generates GOV.UK Design System components.
/// </summary>
public interface IComponentGenerator
{
    /// <summary>
    /// Generates a character count component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateCharacterCount(CharacterCountOptions options);

    /// <summary>
    /// Generates a textarea component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateTextarea(TextareaOptions options);
}
