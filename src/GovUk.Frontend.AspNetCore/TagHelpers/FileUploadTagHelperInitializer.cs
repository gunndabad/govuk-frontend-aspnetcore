using System;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class FileUploadTagHelperInitializer : ITagHelperInitializer<FileUploadTagHelper>
{
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    public FileUploadTagHelperInitializer(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        _optionsAccessor = optionsAccessor;
    }

    public void Initialize(FileUploadTagHelper helper, ViewContext context)
    {
        helper.JavaScriptEnhancements ??= _optionsAccessor.Value.DefaultFileUploadJavaScriptEnhancements;
    }
}
