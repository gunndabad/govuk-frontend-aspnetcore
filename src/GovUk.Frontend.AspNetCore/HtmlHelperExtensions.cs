using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace GovUk.Frontend.AspNetCore;

/// <summary>
/// GOV.UK Frontend extensions for <see cref="IHtmlHelper"/>.
/// </summary>
public static class HtmlHelperExtensions
{
    /// <summary>
    /// Gets the CSP hash for the script that adds a <c>js-enabled</c> CSS class.
    /// </summary>
    /// <param name="htmlHelper">The <see cref="IHtmlHelper"/>.</param>
    /// <returns>A hash to be included in your site's <c>Content-Security-Policy</c> header within the <c>script-src</c> directive.</returns>
    public static string GetJsEnabledScriptCspHash(this IHtmlHelper htmlHelper)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);
        var pageTemplateHelper = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<PageTemplateHelper>();
        return pageTemplateHelper.GetJsEnabledScriptCspHash();
    }

    /// <summary>
    /// Generates the script that adds a <c>js-enabled</c> CSS class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The contents of this property should be inserted at the beginning of the <c>body</c> tag.
    /// </para>
    /// <para>
    /// Use the <see cref="GetJsEnabledScriptCspHash"/> method to retrieve a CSP hash if you are not specifying <paramref name="cspNonce"/>.
    /// </para>
    /// </remarks>
    /// <param name="htmlHelper">The <see cref="IHtmlHelper"/>.</param>
    /// <param name="cspNonce">The CSP nonce attribute to be added to the generated <c>script</c> tag.</param>
    /// <returns><see cref="IHtmlContent"/> containing the <c>script</c> tag.</returns>
    public static IHtmlContent GovUkFrontendJsEnabledScript(this IHtmlHelper htmlHelper, string? cspNonce = null)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);
        var pageTemplateHelper = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<PageTemplateHelper>();
        return pageTemplateHelper.GenerateJsEnabledScript(cspNonce);
    }

    /// <summary>
    /// Generates the script that adds a <c>js-enabled</c> CSS class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The contents of this property should be inserted at the beginning of the <c>body</c> tag.
    /// </para>
    /// <para>
    /// Use the <see cref="GetJsEnabledScriptCspHash"/> method to retrieve a CSP hash if you are not specifying <paramref name="cspNonce"/>.
    /// </para>
    /// </remarks>
    /// <param name="htmlHelper">The <see cref="IHtmlHelper"/>.</param>
    /// <param name="cspNonce">The CSP nonce attribute to be added to the generated <c>script</c> tag.</param>
    /// <param name="appendVersion">Whether the file version should be appended to the <c>src</c> attribute.</param>
    /// <returns><see cref="IHtmlContent"/> containing the <c>script</c> tag.</returns>
    public static IHtmlContent GovUkFrontendScriptImports(this IHtmlHelper htmlHelper, string? cspNonce = null, bool appendVersion = true)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);
        var pageTemplateHelper = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<PageTemplateHelper>();
        return pageTemplateHelper.GenerateScriptImports(cspNonce, appendVersion);
    }

    /// <summary>
    /// Generates the HTML that imports the GOV.UK Frontend library styles.
    /// </summary>
    /// <remarks>
    /// The contents of this property should be inserted in the <c>head</c> tag.
    /// </remarks>
    /// <param name="htmlHelper">The <see cref="IHtmlHelper"/>.</param>
    /// <param name="appendVersion">Whether the file version should be appended to the <c>href</c> attribute.</param>
    /// <returns><see cref="IHtmlContent"/> containing the <c>link</c> tags.</returns>
    public static IHtmlContent GovUkFrontendStyleImports(this IHtmlHelper htmlHelper, bool appendVersion = true)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);
        var pageTemplateHelper = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<PageTemplateHelper>();
        return pageTemplateHelper.GenerateStyleImports(appendVersion);
    }
}
