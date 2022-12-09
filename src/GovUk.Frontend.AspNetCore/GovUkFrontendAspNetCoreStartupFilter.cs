using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace GovUk.Frontend.AspNetCore
{
    internal class GovUkFrontendAspNetCoreStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            Guard.ArgumentNotNull(nameof(next), next);

            return app =>
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new ManifestEmbeddedFileProvider(
                      typeof(GovUkFrontendAspNetCoreStartupFilter).Assembly,
                      root: "Content"),
                    RequestPath = ""  // important - CSS assumes things are available at the root
                });

                next(app);
            };
        }
    }
}
