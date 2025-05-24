using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore;

internal partial class RewriteCompiledAssetsMiddleware
{
    internal const string StaticAssetVersionQueryParamName = "v";

    private readonly RequestDelegate _next;
    private readonly IFileProvider _fileProvider;
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    public RewriteCompiledAssetsMiddleware(
        RequestDelegate next,
        IFileProvider fileProvider,
        IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(fileProvider);
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        _next = next;
        _fileProvider = fileProvider;
        _optionsAccessor = optionsAccessor;
    }

    [GeneratedRegex(@"url\(/assets/(.*?)\)")]
    private static partial Regex GetCssAssetUrlReferencePattern();

    public async Task InvokeAsync(HttpContext context)
    {
        var version = PageTemplateHelper.GovUkFrontendVersion;

        if (_optionsAccessor.Value.CompiledContentPath is PathString compiledContentPath)
        {
            var cssFileName = $"govuk-frontend-{version}.min.css";
            var jsFileName = $"govuk-frontend-{version}.min.js";

            if (context.Request.Path == compiledContentPath + "/" + cssFileName)
            {
                var css = await GetFileContentsAsync(cssFileName);
                css = css.Replace("/*# sourceMappingURL=govuk-frontend.min.css.map */", "");

                if (_optionsAccessor.Value.StaticAssetsContentPath is PathString staticAssetsPath)
                {
                    // If we're hosting the static assets, ensure references in the CSS have the correct base URL.
                    // Also append a version query param so we can send a Cache-Control header with a long duration and 'immutable'.
                    css = GetCssAssetUrlReferencePattern().Replace(
                        css,
                        match =>
                        {
                            var relativePath = match.Groups[1].Value;
                            var withVersionQueryParam = QueryHelpers.AddQueryString(relativePath, StaticAssetVersionQueryParamName, version);
                            return $"url({context.Request.PathBase}{staticAssetsPath}/{withVersionQueryParam})";
                        });
                }

                context.Response.Headers.ContentType = new Microsoft.Extensions.Primitives.StringValues("text/css");
                context.Response.Headers.CacheControl = "Cache-Control: max-age=31536000, immutable";
                await context.Response.WriteAsync(css);
                return;
            }

            if (context.Request.Path == compiledContentPath + "/" + jsFileName)
            {
                var js = await GetFileContentsAsync(jsFileName);
                js = js.Replace("//# sourceMappingURL=govuk-frontend.min.js.map", "");

                context.Response.Headers.ContentType = new Microsoft.Extensions.Primitives.StringValues("text/javascript");
                context.Response.Headers.CacheControl = "Cache-Control: max-age=31536000, immutable";
                await context.Response.WriteAsync(js);
                return;
            }
        }

        await _next(context);
    }

    private async Task<string> GetFileContentsAsync(string fileName)
    {
        var fileInfo = _fileProvider.GetFileInfo(fileName);

        await using var readStream = fileInfo.CreateReadStream();
        using var streamReader = new StreamReader(readStream);
        return await streamReader.ReadToEndAsync();
    }
}
