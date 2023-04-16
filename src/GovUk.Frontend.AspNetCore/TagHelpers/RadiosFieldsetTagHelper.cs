using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the fieldset in a GDS radios component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = RadiosTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = RadiosTagHelper.TagName)]
    [RestrictChildren(
        RadiosFieldsetLegendTagHelper.TagName,
        RadiosFieldsetLegendTagHelper.ShortTagName,
        RadiosItemTagHelper.TagName,
        RadiosItemTagHelper.ShortTagName,
        RadiosItemDividerTagHelper.TagName,
        RadiosItemDividerTagHelper.ShortTagName,
        RadiosTagHelper.HintTagName,
        FormGroupHintTagHelper.ShortTagName,
        RadiosTagHelper.ErrorMessageTagName,
        FormGroupErrorMessageTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.FieldsetElement)]
    public class RadiosFieldsetTagHelper : TagHelper
    {
        internal const string TagName = "govuk-radios-fieldset";
        internal const string ShortTagName = "radios-fieldset";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var radiosContext = context.GetContextItem<RadiosContext>();
            radiosContext.OpenFieldset();

            var fieldsetContext = new RadiosFieldsetContext(output.Attributes.ToAttributeDictionary(), radiosContext.AspFor);

            using (context.SetScopedContextItem(fieldsetContext))
            {
                await output.GetChildContentAsync();
            }

            fieldsetContext.ThrowIfNotComplete();
            radiosContext.CloseFieldset(fieldsetContext);

            output.SuppressOutput();
        }
    }
}
