#nullable enable
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;a&gt; elements.
    /// </summary>
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-action")]
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-controller")]
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-area")]
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-page")]
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-page-handler")]
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-fragment")]
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-host")]
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-protocol")]
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-route")]
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-all-route-data")]
    [HtmlTargetElement("govuk-back-link", Attributes = "asp-route-*")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-action")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-controller")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-area")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-page")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-page-handler")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-fragment")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-host")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-protocol")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-route")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-all-route-data")]
    [HtmlTargetElement("govuk-button-link", Attributes = "asp-route-*")]
    public class AnchorTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper
    {
        /// <inheritdoc/>
        public AnchorTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }
    }
}
