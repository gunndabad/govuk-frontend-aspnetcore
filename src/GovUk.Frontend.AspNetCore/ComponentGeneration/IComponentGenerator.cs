namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// Generates GOV.UK Design System components.
/// </summary>
public interface IComponentGenerator
{
    /// <summary>
    /// Generates an accordion component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateAccordion(AccordionOptions options);

    /// <summary>
    /// Generates a back link component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateBackLink(BackLinkOptions options);

    /// <summary>
    /// Generates a button component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateButton(ButtonOptions options);

    /// <summary>
    /// Generates a cookie banner component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateCookieBanner(CookieBannerOptions options);

    /// <summary>
    /// Generates a details component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateDetails(DetailsOptions options);

    /// <summary>
    /// Generates an error message component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateErrorMessage(ErrorMessageOptions options);

    /// <summary>
    /// Generates an error summary component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateErrorSummary(ErrorSummaryOptions options);

    /// <summary>
    /// Generates an exit this pagecomponent.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateExitThisPage(ExitThisPageOptions options);

    /// <summary>
    /// Generates a fieldset component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateFieldset(FieldsetOptions options);

    /// <summary>
    /// Generates a hint component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateHint(HintOptions options);

    /// <summary>
    /// Generates an inset text component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateInsetText(InsetTextOptions options);

    /// <summary>
    /// Generates a label component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateLabel(LabelOptions options);

    /// <summary>
    /// Generates a pagination component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GeneratePagination(PaginationOptions options);

    /// <summary>
    /// Generates a panel component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GeneratePanel(PanelOptions options);

    /// <summary>
    /// Generates a phase banner component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GeneratePhaseBanner(PhaseBannerOptions options);

    /// <summary>
    /// Generates a skip link component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateSkipLink(SkipLinkOptions options);

    /// <summary>
    /// Generates a tabs component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateTabs(TabsOptions options);

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
    /// Generates a text input component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateTextInput(TextInputOptions options);

    /// <summary>
    /// Generates a warning text component.
    /// </summary>
    /// <returns>An <see cref="HtmlTagBuilder"/> with the component's HTML.</returns>
    HtmlTagBuilder GenerateWarningText(WarningTextOptions options);
}
