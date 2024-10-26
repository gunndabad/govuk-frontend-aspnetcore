using System;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class TitleTagHelperInitializer : ITagHelperInitializer<TitleTagHelper>
{
    private const string DefaultErrorPrefix = "Error:";

    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    public TitleTagHelperInitializer(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        _optionsAccessor = optionsAccessor;
    }

    public void Initialize(TitleTagHelper helper, ViewContext context)
    {
        if (_optionsAccessor.Value.PrependErrorSummaryToForms)
        {
            helper.ErrorPrefix = DefaultErrorPrefix;
        }
    }
}
