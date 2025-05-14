namespace GovUk.Frontend.AspNetCore;

/// <summary>
/// Options that control automatic error summary generation.
/// </summary>
[Flags]
public enum GenerateErrorSummariesOptions
{
    /// <summary>
    /// Don't automatically generate any error summaries.
    /// </summary>
    None = 0,

    /// <summary>
    /// Prepends an error summary to the main element if any components on the page have an error.
    /// </summary>
    PrependToMainElement = 1 << 0,

    /// <summary>
    /// Prepends an error summary to each form element where any of the components within the form have an error.
    /// </summary>
    PrependToFormElements = 1 << 1,

    /// <summary>
    /// Disables the behavior that focuses the error summary when the page loads.
    /// </summary>
    DisableAutoFocus = 1 << 2
}
