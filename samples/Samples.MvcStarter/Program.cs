using GovUk.Frontend.AspNetCore;
using Joonasw.AspNetCore.SecurityHeaders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddGovUkFrontend(options =>
{
    // Un-comment this block if you want to use a CSP nonce instead of hashes
    //options.GetCspNonceForRequest = context =>
    //{
    //    var cspService = context.RequestServices.GetRequiredService<ICspNonceService>();
    //    return cspService.GetNonce();
    //};
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseCsp(csp =>
{
    var pageTemplateHelper = app.Services.GetRequiredService<PageTemplateHelper>();

    csp.ByDefaultAllow
        .FromSelf();

    csp.AllowScripts
        .FromSelf()
        //.AddNonce()
        .From(pageTemplateHelper.GetCspScriptHashes(pathBase: ""));
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
