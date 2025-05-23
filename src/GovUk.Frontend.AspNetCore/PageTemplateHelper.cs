using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore;

/// <summary>
/// Contains methods for generating script and stylesheet imports for the GDS page template.
/// </summary>
public class PageTemplateHelper
{
    private const string JsEnabledScript = "document.body.className += ' js-enabled' + ('noModule' in HTMLScriptElement.prototype ? ' govuk-frontend-supported' : '');";

    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="PageTemplateHelper"/> class.
    /// </summary>
    /// <param name="optionsAccessor">The options.</param>
    public PageTemplateHelper(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        _optionsAccessor = optionsAccessor;
    }

    /// <summary>
    /// Gets the version of the GOV.UK Frontend library.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static string GovUkFrontendVersion => GovUkFrontendInfo.Version;

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
    /// <param name="cspNonce">The CSP nonce attribute to be added to the generated <c>script</c> tag.</param>
    /// <returns><see cref="IHtmlContent"/> containing the <c>script</c> tag.</returns>
    public IHtmlContent GenerateJsEnabledScript(string? cspNonce = null)
    {
        var tagBuilder = new TagBuilder("script");

        if (cspNonce is not null)
        {
            tagBuilder.MergeAttribute("nonce", cspNonce);
        }

        tagBuilder.InnerHtml.AppendHtml(new HtmlString(JsEnabledScript));

        return tagBuilder;
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
    /// <param name="cspNonce">The CSP nonce attribute to be added to the generated <c>script</c> tag.</param>
    /// <returns><see cref="IHtmlContent"/> containing the <c>script</c> tag.</returns>
    public IHtmlContent GenerateScriptImports(string? cspNonce = null) =>
        GenerateScriptImports(pathBase: "", cspNonce);

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
    /// <param name="pathBase">The base path that the script is hosted under.</param>
    /// <param name="cspNonce">The CSP nonce attribute to be added to the generated <c>script</c> tag.</param>
    /// <returns><see cref="IHtmlContent"/> containing the <c>script</c> tag.</returns>
    public IHtmlContent GenerateScriptImports(PathString pathBase, string? cspNonce)
    {
        var compiledContentPath = _optionsAccessor.Value.CompiledContentPath;
        if (compiledContentPath is null)
        {
            throw new InvalidOperationException($"Cannot generate script imports when {nameof(GovUkFrontendAspNetCoreOptions.CompiledContentPath)} is null.");
        }

        var htmlContentBuilder = new HtmlContentBuilder();
        htmlContentBuilder.AppendHtml(GenerateImportScript());
        htmlContentBuilder.AppendLine();
        htmlContentBuilder.AppendHtml(GenerateInitScript());
        htmlContentBuilder.AppendLine();
        return htmlContentBuilder;

        TagBuilder GenerateImportScript()
        {
            var src = GetScriptSrc(pathBase, compiledContentPath);

            var tagBuilder = new TagBuilder("script");
            tagBuilder.MergeAttribute("type", "module");
            tagBuilder.MergeAttribute("src", src);
            return tagBuilder;
        }

        TagBuilder GenerateInitScript()
        {
            var tagBuilder = new TagBuilder("script");
            tagBuilder.MergeAttribute("type", "module");

            if (cspNonce is not null)
            {
                tagBuilder.MergeAttribute("nonce", cspNonce);
            }

            tagBuilder.InnerHtml.AppendHtml(new HtmlString(GetInitScriptContents(pathBase)));

            return tagBuilder;
        }
    }

    /// <summary>
    /// Generates the HTML that imports the GOV.UK Frontend library styles.
    /// </summary>
    /// <remarks>
    /// The contents of this property should be inserted in the <c>head</c> tag.
    /// </remarks>
    /// <returns><see cref="IHtmlContent"/> containing the <c>link</c> tags.</returns>
    public IHtmlContent GenerateStyleImports() => GenerateStyleImports(pathBase: "");

    /// <summary>
    /// Generates the HTML that imports the GOV.UK Frontend library styles.
    /// </summary>
    /// <remarks>
    /// The contents of this property should be inserted in the <c>head</c> tag.
    /// </remarks>
    /// <param name="pathBase">The base path that the stylesheet is hosted under.</param>
    /// <returns><see cref="IHtmlContent"/> containing the <c>link</c> tags.</returns>
    public IHtmlContent GenerateStyleImports(PathString pathBase)
    {
        var compiledContentPath = _optionsAccessor.Value.CompiledContentPath;
        if (compiledContentPath is null)
        {
            throw new InvalidOperationException($"Cannot generate style imports when {nameof(GovUkFrontendAspNetCoreOptions.CompiledContentPath)} is null.");
        }

        var fileName = $"govuk-frontend-{GovUkFrontendVersion}.min.css";
        var href = $"{pathBase}{compiledContentPath}/{fileName}";

        return new HtmlString($"<link href=\"{href}\" rel=\"stylesheet\">");
    }

    /// <summary>
    /// Gets all the CSP hashes for the inline scripts used in the page template.
    /// </summary>
    /// <returns>A list of hashes to be included in your site's <c>Content-Security-Policy</c> header within the <c>script-src</c> directive.</returns>
    public string GetCspScriptHashes() => GetCspScriptHashes(pathBase: "");

    /// <summary>
    /// Gets all the CSP hashes for the inline scripts used in the page template.
    /// </summary>
    /// <param name="pathBase">The base path that the script is hosted under.</param>
    /// <returns>A list of hashes to be included in your site's <c>Content-Security-Policy</c> header within the <c>script-src</c> directive.</returns>
    public string GetCspScriptHashes(PathString pathBase) => $"{GetInitScriptCspHash(pathBase)} {GetJsEnabledScriptCspHash()}";

    /// <summary>
    /// Gets the CSP hash for the script that adds a <c>js-enabled</c> CSS class.
    /// </summary>
    /// <returns>A hash to be included in your site's <c>Content-Security-Policy</c> header within the <c>script-src</c> directive.</returns>
    public string GetJsEnabledScriptCspHash() => GenerateCspHash(JsEnabledScript);

    /// <summary>
    /// Gets the CSP hash for the GOV.UK Frontend initialization script.
    /// </summary>
    /// <returns>A hash to be included in your site's <c>Content-Security-Policy</c> header within the <c>script-src</c> directive.</returns>
    public string GetInitScriptCspHash() => GetInitScriptCspHash(pathBase: "");

    /// <summary>
    /// Gets the CSP hash for the GOV.UK Frontend initialization script.
    /// </summary>
    /// <param name="pathBase">The base path that the script is hosted under.</param>
    /// <returns>A hash to be included in your site's <c>Content-Security-Policy</c> header within the <c>script-src</c> directive.</returns>
    public string GetInitScriptCspHash(PathString pathBase) =>
        GenerateCspHash(GetInitScriptContents(pathBase));

    private string GetInitScriptContents(PathString pathBase)
    {
        if (_optionsAccessor.Value.CompiledContentPath is null)
        {
            throw new InvalidOperationException($"Cannot generate scripts when {nameof(GovUkFrontendAspNetCoreOptions.CompiledContentPath)} is null.");
        }

        var compiledContentPath = $"{pathBase}{_optionsAccessor.Value.CompiledContentPath}";

        var fileName = GetScriptFileName();
        return $"\nimport {{ initAll }} from '{compiledContentPath}/{fileName}'\ninitAll()\n";
    }

    private static string GenerateCspHash(string value)
    {
        using var algo = SHA256.Create();
        var hash = algo.ComputeHash(Encoding.UTF8.GetBytes(value));
        return $"'sha256-{Convert.ToBase64String(hash)}'";
    }

    private static string GetScriptFileName() => $"govuk-frontend-{GovUkFrontendVersion}.min.js";

    private static string GetScriptSrc(PathString pathBase, PathString? compiledContentPath)
    {
        var fileName = GetScriptFileName();
        return $"{pathBase}{compiledContentPath}/{fileName}";
    }
}
