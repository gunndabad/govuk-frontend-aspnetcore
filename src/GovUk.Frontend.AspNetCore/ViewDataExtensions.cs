using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore;

/// <summary>
/// Extensions for <see cref="ViewDataDictionary"/>.
/// </summary>
public static class ViewDataExtensions
{
    private const string PageHasErrorsKey = $"GovUk.Frontend.AspNetCore.{nameof(PageHasErrorsKey)}";

    /// <summary>
    /// Gets whether the page has errors.
    /// </summary>
    /// <param name="viewData">The <see cref="ViewDataDictionary"/>.</param>
    /// <returns><c>true</c> if the page has errors otherwise <c>false</c>.</returns>
    public static bool GetPageHasErrors(this ViewDataDictionary viewData)
    {
        Guard.ArgumentNotNull(nameof(viewData), viewData);

        return viewData.TryGetValue(PageHasErrorsKey, out var hasErrorsObj) && (bool)hasErrorsObj!;
    }

    /// <summary>
    /// Sets whether the page has errors.
    /// </summary>
    /// <param name="viewData">The <see cref="ViewDataDictionary"/>.</param>
    /// <param name="pageHasErrors">Whether the page has errors.</param>
    public static void SetPageHasErrors(this ViewDataDictionary viewData, bool pageHasErrors)
    {
        Guard.ArgumentNotNull(nameof(viewData), viewData);

        viewData[PageHasErrorsKey] = pageHasErrors;
    }
}
