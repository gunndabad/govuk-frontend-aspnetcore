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

    /// <summary>
    /// Generates a button component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateButton(ButtonOptions options);

    /// <summary>
    /// Generates a cookie banner component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateCookieBanner(CookieBannerOptions options);

    /// <summary>
    /// Generates a details component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateDetails(DetailsOptions options);

    /// <summary>
    /// Generates an error message component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateErrorMessage(ErrorMessageOptions options);

    /// <summary>
    /// Generates an error summary component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateErrorSummary(ErrorSummaryOptions options);

    /// <summary>
    /// Generates a fieldset component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateFieldset(FieldsetOptions options);

    /// <summary>
    /// Generates a hint component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateHint(HintOptions options);

    /// <summary>
    /// Generates an inset text component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateInsetText(InsetTextOptions options);

    /// <summary>
    /// Generates a label component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateLabel(LabelOptions options);

    /// <summary>
    /// Generates a phase banner component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GeneratePhaseBanner(PhaseBannerOptions options);

    /// <summary>
    /// Generates a tag component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateTag(TagOptions options);

    /// <summary>
    /// Generates a text input component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateTextInput(TextInputOptions options);

    /// <summary>
    /// Generates a warning text component.
    /// </summary>
    /// <returns>An <see cref="HtmlTag"/> with the component's HTML.</returns>
    HtmlTag GenerateWarningText(WarningTextOptions options);
}
