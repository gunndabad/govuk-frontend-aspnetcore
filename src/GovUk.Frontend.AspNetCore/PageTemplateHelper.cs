using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore;

/// <summary>
/// Contains methods for generating script and stylesheet imports for the GDS page template.
/// </summary>
public class PageTemplateHelper
{
    internal const string JsEnabledScript = "document.body.className += ' js-enabled' + ('noModule' in HTMLScriptElement.prototype ? ' govuk-frontend-supported' : '');";
    private const string VersionQueryParameterName = "v";

    private static readonly ConcurrentDictionary<string, string> _embeddedResourceFileVersionCache = new();

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

    private static readonly string _frontendVersion = GetGovUkFrontendVersion();

    /// <summary>
    /// Gets the version of the GOV.UK Frontend library.
    /// </summary>
    public static string GovUkFrontendVersion => _frontendVersion;

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
    public IHtmlContent GenerateScriptImports(string? cspNonce = null) => GenerateScriptImports(cspNonce, appendVersion: false);

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
    /// <param name="appendVersion">Whether the file version should be appended to the <c>src</c> attribute.</param>
    /// <returns><see cref="IHtmlContent"/> containing the <c>script</c> tag.</returns>
    public IHtmlContent GenerateScriptImports(string? cspNonce = null, bool appendVersion = false)
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
            var src = $"{compiledContentPath}/all.min.js";
            if (appendVersion)
            {
                var version = _embeddedResourceFileVersionCache.GetOrAdd("Content/Compiled/all.min.js", path => GetEmbeddedResourceVersion(path));
                src = QueryHelpers.AddQueryString(src, VersionQueryParameterName, version);
            }

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

            tagBuilder.InnerHtml.AppendHtml(new HtmlString(GetInitScriptContents()));

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
    public IHtmlContent GenerateStyleImports() => GenerateStyleImports(appendVersion: false);

    /// <summary>
    /// Generates the HTML that imports the GOV.UK Frontend library styles.
    /// </summary>
    /// <remarks>
    /// The contents of this property should be inserted in the <c>head</c> tag.
    /// </remarks>
    /// <param name="appendVersion">Whether the file version should be appended to the <c>src</c> attribute.</param>
    /// <returns><see cref="IHtmlContent"/> containing the <c>link</c> tags.</returns>
    public IHtmlContent GenerateStyleImports(bool appendVersion)
    {
        var compiledContentPath = _optionsAccessor.Value.CompiledContentPath;
        if (compiledContentPath is null)
        {
            throw new InvalidOperationException($"Cannot generate style imports when {nameof(GovUkFrontendAspNetCoreOptions.CompiledContentPath)} is null.");
        }

        var href = $"{compiledContentPath}/all.min.css";
        if (appendVersion)
        {
            var version = _embeddedResourceFileVersionCache.GetOrAdd("Content/Compiled/all.min.css", path => GetEmbeddedResourceVersion(path));
            href = QueryHelpers.AddQueryString(href, VersionQueryParameterName, version);
        }

        return new HtmlString($"<link href=\"{href}\" rel=\"stylesheet\">");
    }

    /// <summary>
    /// Gets all the CSP hashes for the inline scripts used in the page template.
    /// </summary>
    /// <returns>A list of hashes to be included in your site's <c>Content-Security-Policy</c> header within the <c>script-src</c> directive.</returns>
    public string GetCspScriptHashes() => $"{GetInitScriptCspHash()} {GetJsEnabledScriptCspHash()}";

    /// <summary>
    /// Gets the CSP hash for the script that adds a <c>js-enabled</c> CSS class.
    /// </summary>
    /// <returns>A hash to be included in your site's <c>Content-Security-Policy</c> header within the <c>script-src</c> directive.</returns>
    public string GetJsEnabledScriptCspHash() => GenerateCspHash(JsEnabledScript);

    /// <summary>
    /// Gets the CSP hash for the GOV.UK Frontend initialization script.
    /// </summary>
    /// <returns>A hash to be included in your site's <c>Content-Security-Policy</c> header within the <c>script-src</c> directive.</returns>
    public string GetInitScriptCspHash() => GetInitScriptCspHash(GetInitScriptContents());

    private string GetInitScriptContents()
    {
        var compiledContentPath = _optionsAccessor.Value.CompiledContentPath;
        if (compiledContentPath is null)
        {
            throw new InvalidOperationException($"Cannot generate scripts when {nameof(GovUkFrontendAspNetCoreOptions.CompiledContentPath)} is null.");
        }

        return $"\nimport {{ initAll }} from '{compiledContentPath}/all.min.js'\ninitAll()\n";
    }

    private string GetInitScriptCspHash(string initScript) => GenerateCspHash(initScript);

    private static string GetGovUkFrontendVersion() =>
        typeof(PageTemplateHelper).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "GovUkFrontendVersion")
            .Value!;

    private static string GenerateCspHash(string value)
    {
        using var algo = SHA256.Create();
        var hash = algo.ComputeHash(Encoding.UTF8.GetBytes(value));
        return $"'sha256-{Convert.ToBase64String(hash)}'";
    }

    private static string GetEmbeddedResourceVersion(string path)
    {
        using var resourceStream = typeof(PageTemplateHelper).Assembly.GetManifestResourceStream($"{path}") ??
            throw new ArgumentException($"Could not find resource: '{path}'.", nameof(path));
        using var ms = new MemoryStream();
        resourceStream.CopyTo(ms);
        var hash = SHA256.HashData(ms.ToArray());
        return WebEncoders.Base64UrlEncode(hash);
    }
}
