using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the fieldset in a GDS checkboxes component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = CheckboxesTagHelper.TagName)]
    [HtmlTargetElement(ShortTagName, ParentTag = CheckboxesTagHelper.TagName)]
    [RestrictChildren(
        CheckboxesFieldsetLegendTagHelper.TagName,
        CheckboxesFieldsetLegendTagHelper.ShortTagName,
        CheckboxesItemTagHelper.TagName,
        CheckboxesItemTagHelper.ShortTagName,
        CheckboxesItemDividerTagHelper.TagName,
        CheckboxesItemDividerTagHelper.ShortTagName,
        CheckboxesTagHelper.HintTagName,
        FormGroupHintTagHelper.ShortTagName,
        CheckboxesTagHelper.ErrorMessageTagName,
        FormGroupErrorMessageTagHelper.ShortTagName)]
    [OutputElementHint(ComponentGenerator.FieldsetElement)]
    public class CheckboxesFieldsetTagHelper : TagHelper
    {
        internal const string TagName = "govuk-checkboxes-fieldset";
        internal const string ShortTagName = "checkboxes-fieldset";  // 'fieldset' clashes with FieldsetTagHelper

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var checkboxesContext = context.GetContextItem<CheckboxesContext>();
            checkboxesContext.OpenFieldset();

            var fieldsetContext = new CheckboxesFieldsetContext(output.Attributes.ToAttributeDictionary(), checkboxesContext.AspFor);

            using (context.SetScopedContextItem(fieldsetContext))
            {
                await output.GetChildContentAsync();
            }

            fieldsetContext.ThrowIfNotComplete();
            checkboxesContext.CloseFieldset(fieldsetContext);

            output.SuppressOutput();
        }
    }
}
