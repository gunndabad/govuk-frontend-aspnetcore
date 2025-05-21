using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class ButtonTagHelperInitializer : ITagHelperInitializer<ButtonTagHelper>
{
    private readonly IOptions<GovUkFrontendAspNetCoreOptions> _optionsAccessor;

    public ButtonTagHelperInitializer(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
    {
        ArgumentNullException.ThrowIfNull(optionsAccessor);
        _optionsAccessor = optionsAccessor;
    }

    public void Initialize(ButtonTagHelper helper, ViewContext context)
    {
        helper.PreventDoubleClick ??= _optionsAccessor.Value.DefaultButtonPreventDoubleClick;
    }
}
