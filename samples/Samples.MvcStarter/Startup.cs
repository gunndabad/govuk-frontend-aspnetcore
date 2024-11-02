using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore;
using Joonasw.AspNetCore.SecurityHeaders;
using Joonasw.AspNetCore.SecurityHeaders.Csp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Samples.MvcStarter;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddGovUkFrontend(options =>
        {
            // Un-comment this block if you want to use a CSP nonce instead of hashes
            //options.GetCspNonceForRequest = context =>
            //{
            //    var cspService = context.RequestServices.GetRequiredService<ICspNonceService>();
            //    return cspService.GetNonce();
            //};
        });

        services.AddCsp(nonceByteAmount: 32);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseCsp(csp =>
        {
            var pageTemplateHelper = app.ApplicationServices.GetRequiredService<PageTemplateHelper>();

            csp.ByDefaultAllow.FromSelf();

            csp.AllowScripts.FromSelf()
                //.AddNonce()
                .From(pageTemplateHelper.GetCspScriptHashes(pathBase: ""));
        });

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}
