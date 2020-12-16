#nullable enable
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;button&gt; elements.
    /// </summary>
    [HtmlTargetElement(ButtonTagHelper.TagName, Attributes = "asp-action")]
    [HtmlTargetElement(ButtonTagHelper.TagName, Attributes = "asp-controller")]
    [HtmlTargetElement(ButtonTagHelper.TagName, Attributes = "asp-area")]
    [HtmlTargetElement(ButtonTagHelper.TagName, Attributes = "asp-page")]
    [HtmlTargetElement(ButtonTagHelper.TagName, Attributes = "asp-page-handler")]
    [HtmlTargetElement(ButtonTagHelper.TagName, Attributes = "asp-fragment")]
    [HtmlTargetElement(ButtonTagHelper.TagName, Attributes = "asp-route")]
    [HtmlTargetElement(ButtonTagHelper.TagName, Attributes = "asp-all-route-data")]
    [HtmlTargetElement(ButtonTagHelper.TagName, Attributes = "asp-route-*")]
    public class FormActionTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper
    {
        /// <inheritdoc />
        public FormActionTagHelper(IUrlHelperFactory urlHelperFactory)
            : base(urlHelperFactory)
        {
        }
    }
}
