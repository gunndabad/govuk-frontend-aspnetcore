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
    /// <inheritdoc cref="PageTemplateHelper.GenerateJsEnabledScript(string?)"/>
    public static IHtmlContent GovUkFrontendJsEnabledScript(this IHtmlHelper htmlHelper, string? cspNonce = null)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);
        var pageTemplateHelper = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<PageTemplateHelper>();
        return pageTemplateHelper.GenerateJsEnabledScript(cspNonce);
    }

    /// <inheritdoc cref="PageTemplateHelper.GenerateScriptImports(string?)"/>
    public static IHtmlContent GovUkFrontendScriptImports(this IHtmlHelper htmlHelper, string? cspNonce = null)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);
        var pageTemplateHelper = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<PageTemplateHelper>();
        return pageTemplateHelper.GenerateScriptImports(cspNonce);
    }

    /// <inheritdoc cref="PageTemplateHelper.GenerateStyleImports"/>
    public static IHtmlContent GovUkFrontendStyleImports(this IHtmlHelper htmlHelper)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);
        var pageTemplateHelper = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<PageTemplateHelper>();
        return pageTemplateHelper.GenerateStyleImports();
    }
}
