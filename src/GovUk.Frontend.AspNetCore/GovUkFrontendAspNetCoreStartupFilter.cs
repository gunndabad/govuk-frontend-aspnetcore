using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore;

internal class GovUkFrontendAspNetCoreStartupFilter : IStartupFilter
{
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    public GovUkFrontendAspNetCoreStartupFilter(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        _optionsAccessor = optionsAccessor;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        Guard.ArgumentNotNull(nameof(next), next);

        return app =>
        {
            if (_optionsAccessor.Value.CompiledContentPath is PathString compiledContentPath)
            {
                var fileProvider = new ManifestEmbeddedFileProvider(
                    typeof(GovUkFrontendAspNetCoreStartupFilter).Assembly,
                    root: "Content/Compiled"
                );

                app.UseMiddleware<RewriteCssUrlsMiddleware>(fileProvider);

                app.UseStaticFiles(
                    new StaticFileOptions() { FileProvider = fileProvider, RequestPath = compiledContentPath }
                );
            }

            if (_optionsAccessor.Value.StaticAssetsContentPath is PathString assetsContentPath)
            {
                var fileProvider = new ManifestEmbeddedFileProvider(
                    typeof(GovUkFrontendAspNetCoreStartupFilter).Assembly,
                    root: "Content/Assets"
                );

                app.UseStaticFiles(
                    new StaticFileOptions() { FileProvider = fileProvider, RequestPath = assetsContentPath }
                );
            }

            next(app);
        };
    }
}
