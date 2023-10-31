using HtmlTags;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// Generates GOV.UK Design System components.
/// </summary>
public interface IComponentGenerator
{
    /// <summary>
    /// Generates a back link component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateBackLink(BackLinkOptions options);
}
