using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore;

internal class RewriteCompiledAssetsMiddleware
{
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

    public async Task InvokeAsync(HttpContext context)
    {
        if (_optionsAccessor.Value.CompiledContentPath is PathString compiledContentPath &&
            _optionsAccessor.Value.StaticAssetsContentPath is PathString staticAssetsPath)
        {
            if (context.Request.Path == compiledContentPath + "/all.min.css")
            {
                var fileInfo = _fileProvider.GetFileInfo("all.min.css");

                using var readStream = fileInfo.CreateReadStream();
                using var streamReader = new StreamReader(readStream);
                var css = await streamReader.ReadToEndAsync();

                css = css.Replace("url(/assets/", $"url({context.Request.PathBase}{staticAssetsPath}/");
                css = css.Replace("/*# sourceMappingURL=govuk-frontend.min.css.map */", "");

                context.Response.Headers.ContentType = new Microsoft.Extensions.Primitives.StringValues("text/css");
                await context.Response.WriteAsync(css);
                return;
            }

            if (context.Request.Path == compiledContentPath + "/all.min.js")
            {
                var fileInfo = _fileProvider.GetFileInfo("all.min.js");

                using var readStream = fileInfo.CreateReadStream();
                using var streamReader = new StreamReader(readStream);
                var css = await streamReader.ReadToEndAsync();

                css = css.Replace("//# sourceMappingURL=govuk-frontend.min.js.map", "");

                context.Response.Headers.ContentType = new Microsoft.Extensions.Primitives.StringValues("text/javascript");
                await context.Response.WriteAsync(css);
                return;
            }
        }

        await _next(context);
    }
}
