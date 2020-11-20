using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-input")]
    [RestrictChildren("govuk-input-label", "govuk-input-hint", "govuk-input-error-message", "govuk-input-prefix", "govuk-input-suffix")]
    public class InputTagHelper : FormGroupTagHelperBase
    {
        private const string AttributesPrefix = "input-";
        private const string AutocompleteAttributeName = "autocomplete";
        private const string IdAttributeName = "id";
        private const string IsDisabledAttributeName = "disabled";
        private const string InputModeAttributeName = "inputmode";
        private const string PatternAttributeName = "pattern";
        private const string SpellcheckAttributeName = "spellcheck";
        private const string TypeAttributeName = "type";
        private const string ValueAttributeName = "value";

        private string _value;
        private bool _valueSpecified = false;

        public InputTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        [HtmlAttributeName(AutocompleteAttributeName)]
        public string Autocomplete { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(InputModeAttributeName)]
        public string InputMode { get; set; }

        [HtmlAttributeName(IsDisabledAttributeName)]
        public bool IsDisabled { get; set; }

        [HtmlAttributeName(PatternAttributeName)]
        public string Pattern { get; set; }

        [HtmlAttributeName(SpellcheckAttributeName)]
        public bool? Spellcheck { get; set; }

        [HtmlAttributeName(TypeAttributeName)]
        public string Type { get; set; } = ComponentDefaults.Input.Type;

        [HtmlAttributeName(ValueAttributeName)]
        public string Value
        {
            get => _value;
            set
            { 
                _value = value;
                _valueSpecified = true;
            }
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var inputContext = new InputContext();
            using (context.SetScopedContextItem(typeof(InputContext), inputContext))
            {
                await base.ProcessAsync(context, output);
            }
        }

        protected override TagBuilder GenerateElement(
            TagHelperContext context,
            FormGroupBuilder builder,
            FormGroupElementContext elementContext)
        {
            if (AspFor == null && !_valueSpecified)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{AspForAttributeName}' and '{ValueAttributeName}' attributes must be specified.");
            }

            var inputContext = (InputContext)context.Items[typeof(InputContext)];

            var resolvedValue = Value ??
                (AspFor != null ? Generator.GetModelValue(ViewContext, AspFor.ModelExplorer, AspFor.Name) : null);

            return Generator.GenerateInput(
                elementContext.HaveError,
                ResolvedId,
                ResolvedName,
                Type,
                resolvedValue,
                DescribedBy,
                Autocomplete,
                Pattern,
                InputMode,
                Spellcheck,
                IsDisabled,
                Attributes,
                inputContext.Prefix?.content,
                inputContext.Prefix?.attributes,
                inputContext.Suffix?.content,
                inputContext.Suffix?.attributes);
        }

        protected override string GetIdPrefix() => Id;
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

    [HtmlTargetElement("govuk-input-prefix", ParentTag = "govuk-input")]
    public class InputPrefixTagHelper : FormGroupErrorMessageTagHelperBase
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var inputContext = (InputContext)context.Items[typeof(InputContext)];

            var content = await output.GetChildContentAsync();

            inputContext.SetPrefix(output.Attributes.ToAttributesDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-input-suffix", ParentTag = "govuk-input")]
    public class InputSuffixTagHelper : FormGroupErrorMessageTagHelperBase
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var inputContext = (InputContext)context.Items[typeof(InputContext)];

            var content = await output.GetChildContentAsync();

            inputContext.SetSuffix(output.Attributes.ToAttributesDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }

    internal class InputContext
    {
        public (IDictionary<string, string> attributes, IHtmlContent content)? Prefix { get; private set; }
        public (IDictionary<string, string> attributes, IHtmlContent content)? Suffix { get; private set; }

        public void SetPrefix(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Prefix != null)
            {
                throw new InvalidOperationException("Prefix content has already been set.");
            }

            Prefix = (attributes, content);
        }

        public void SetSuffix(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Suffix != null)
            {
                throw new InvalidOperationException("Suffix content has already been set.");
            }

            Suffix = (attributes, content);
        }
    }
}
