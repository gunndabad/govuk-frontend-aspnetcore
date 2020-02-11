using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-input")]
    [RestrictChildren("govuk-input-label", "govuk-input-hint", "govuk-input-error-message")]
    public class InputTagHelper : FormGroupTagHelperBase
    {
        private const string AutocompleteAttributeName = "autocomplete";
        private const string InputModeAttributeName = "inputmode";
        private const string PatternAttributeName = "pattern";
        private const string TypeAttributeName = "type";
        private const string ValueAttributeName = "value";
        
        public InputTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(AutocompleteAttributeName)]
        public string Autocomplete { get; set; }

        [HtmlAttributeName(InputModeAttributeName)]
        public string InputMode { get; set; }

        [HtmlAttributeName(PatternAttributeName)]
        public string Pattern { get; set; }

        [HtmlAttributeName(TypeAttributeName)]
        public string Type { get; set; }

        [HtmlAttributeName(ValueAttributeName)]
        public string Value { get; set; }

        private protected override IHtmlContent CreateElement(FormGroupBuilder builder, FormGroupElementContext context)
        {
            var resolvedValue = Value ??
                (AspFor != null ? Generator.GetModelValue(ViewContext, AspFor.ModelExplorer, AspFor.Name) : null);

            return Generator.GenerateInput(
                context.HaveError,
                context.ElementId,
                context.ElementName,
                Type,
                resolvedValue,
                context.DescribedBy,
                Autocomplete,
                Pattern,
                InputMode);
        }
    }

    [HtmlTargetElement("govuk-input-label", ParentTag = "govuk-input")]
    public class InputLabelTagHelper : FormGroupLabelTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-input-hint", ParentTag = "govuk-input")]
    public class InputHintTagHelper : FormGroupHintTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-input-error-message", ParentTag = "govuk-input")]
    public class InputErrorMessageTagHelper : FormGroupErrorMessageTagHelperBase
    {
    }
}
