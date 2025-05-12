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
    ValueTask<IHtmlContent> GenerateAccordionAsync(AccordionOptions2 options);

    /// <summary>
    /// Generates a back link component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateBackLinkAsync(BackLinkOptions options);

    /// <summary>
    /// Generates a breadcrumbs component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateBreadcrumbsAsync(BreadcrumbsOptions options);

    /// <summary>
    /// Generates a button component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateButtonAsync(ButtonOptions options);

    /// <summary>
    /// Generates a character count component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateCharacterCountAsync(CharacterCountOptions options);

    /// <summary>
    /// Generates a checkboxes component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateCheckboxesAsync(CheckboxesOptions options);

    /// <summary>
    /// Generates a cookie banner component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateCookieBannerAsync(CookieBannerOptions options);

    /// <summary>
    /// Generates a details component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateDetailsAsync(DetailsOptions options);

    /// <summary>
    /// Generates an error message component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateErrorMessageAsync(ErrorMessageOptions2 options);

    /// <summary>
    /// Generates an error summary component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateErrorSummaryAsync(ErrorSummaryOptions options);

    /// <summary>
    /// Generates a fieldset component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateFieldsetAsync(FieldsetOptions2 options);

    /// <summary>
    /// Generates a file upload component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateFileUploadAsync(FileUploadOptions options);

    /// <summary>
    /// Generates a footer component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateFooterAsync(FooterOptions options);

    /// <summary>
    /// Generates a header component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateHeaderAsync(HeaderOptions options);

    /// <summary>
    /// Generates a hint component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateHintAsync(HintOptions2 options);

    /// <summary>
    /// Generates a label component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateLabelAsync(LabelOptions2 options);

    /// <summary>
    /// Generates a pagination component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GeneratePaginationAsync(PaginationOptions options);

    /// <summary>
    /// Generates a service navigation component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateServiceNavigationAsync(ServiceNavigationOptions options);

    /// <summary>
    /// Generates a summary list component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateSummaryListAsync(SummaryListOptions options);

    /// <summary>
    /// Generates a tag component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateTagAsync(TagOptions options);

    /// <summary>
    /// Generates a warning text component.
    /// </summary>
    /// <returns>A <see cref="string"/> with the component's HTML.</returns>
    ValueTask<IHtmlContent> GenerateWarningTextAsync(WarningTextOptions options);
}
