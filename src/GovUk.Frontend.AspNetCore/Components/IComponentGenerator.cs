using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.Components;

/// <summary>
/// Generates GOV.UK Design System components.
/// </summary>
public interface IComponentGenerator
{
    /// <summary>
    /// Generates an accordion component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateAccordion(AccordionOptions2 options);

    /// <summary>
    /// Generates a back link component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateBackLink(BackLinkOptions options);

    /// <summary>
    /// Generates a breadcrumbs component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateBreadcrumbs(BreadcrumbsOptions options);

    /// <summary>
    /// Generates a button component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateButton(ButtonOptions2 options);

    /// <summary>
    /// Generates a checkboxes component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateCheckboxes(CheckboxesOptions2 options);

    /// <summary>
    /// Generates a cookie banner component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateCookieBanner(CookieBannerOptions options);

    /// <summary>
    /// Generates an error message component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateErrorMessage(ErrorMessageOptions2 options);

    /// <summary>
    /// Generates a fieldset component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateFieldset(FieldsetOptions2 options);

    /// <summary>
    /// Generates a file upload component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateFileUpload(FileUploadOptions options);

    /// <summary>
    /// Generates a hint component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateHint(HintOptions2 options);

    /// <summary>
    /// Generates a label component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateLabel(LabelOptions2 options);

    /// <summary>
    /// Generates a warning text component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateWarningText(WarningTextOptions options);
}
