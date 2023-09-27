using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelperComponents;

/// <summary>
/// A <see cref="TagHelperComponent"/> that adds imports and initialization scripts for the GOV.UK Frontend library.
/// </summary>
public class GdsImportsTagHelperComponent : TagHelperComponent
{
    private readonly GovUkFrontendAspNetCoreOptions _options;
    private readonly PageTemplateHelper _pageTemplateHelper;

    /// <summary>
    /// Creates a new <see cref="GdsImportsTagHelperComponent"/>.
    /// </summary>
    public GdsImportsTagHelperComponent(
        IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor,
        PageTemplateHelper pageTemplateHelper)
    {
        _options = Guard.ArgumentNotNull(nameof(optionsAccessor), optionsAccessor).Value;
        _pageTemplateHelper = Guard.ArgumentNotNull(nameof(pageTemplateHelper), pageTemplateHelper);
    }

    /// <inheritdoc/>
    public override int Order => 1;

    /// <summary>
    /// Gets the <see cref="ViewContext"/> of the executing view.
    /// </summary>
    [ViewContext]
    [HtmlAttributeNotBound]
    [DisallowNull]
    public ViewContext? ViewContext { get; set; }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!_options.AddImportsToHtml)
        {
            return;
        }

        if (ViewContext!.ViewData.TryGetValue(NoAppendHtmlSnippetsMarker.ViewDataKey, out var noAppendHtmlSnippetsMarkerObj) &&
            noAppendHtmlSnippetsMarkerObj == NoAppendHtmlSnippetsMarker.Instance)
        {
            return;
        }

        var cspNonce = _options.GetCspNonceForRequest?.Invoke(ViewContext.HttpContext);

        if (string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase))
        {
            output.PostContent.AppendHtml(_pageTemplateHelper.GenerateStyleImports());
            output.PostContent.AppendHtml("\n");
        }
        else if (string.Equals(context.TagName, "body", StringComparison.OrdinalIgnoreCase))
        {
            output.AddClass("govuk-template__body", HtmlEncoder.Default);

            output.PreContent.AppendHtml("\n");
            output.PreContent.AppendHtml(_pageTemplateHelper.GenerateJsEnabledScript(cspNonce));

            output.PostContent.AppendHtml(_pageTemplateHelper.GenerateScriptImports(cspNonce));
            output.PostContent.AppendHtml("\n");
        }
    }
}
