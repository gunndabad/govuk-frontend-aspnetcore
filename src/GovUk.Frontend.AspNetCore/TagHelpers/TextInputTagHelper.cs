#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS input component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [RestrictChildren(LabelTagName, HintTagName, ErrorMessageTagName, TextInputPrefixTagHelper.TagName, TextInputSuffixTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.FormGroupElement)]
    public class TextInputTagHelper : FormGroupTagHelperBase
    {
        internal const string ErrorMessageTagName = "govuk-input-error-message";
        internal const string HintTagName = "govuk-input-hint";
        internal const string LabelTagName = "govuk-input-label";
        internal const string TagName = "govuk-input";

        private const string AttributesPrefix = "input-";
        private const string AutocompleteAttributeName = "autocomplete";
        private const string DescribedByAttributeName = "described-by";
        private const string DisabledAttributeName = "disabled";
        private const string IdAttributeName = "id";
        private const string InputModeAttributeName = "inputmode";
        private const string NameAttributeName = "name";
        private const string PatternAttributeName = "pattern";
        private const string SpellcheckAttributeName = "spellcheck";
        private const string TypeAttributeName = "type";
        private const string ValueAttributeName = "value";

        private string? _type = ComponentGenerator.InputDefaultType;
        private string? _value;
        private bool _valueSpecified;

        /// <summary>
        /// Creates an <see cref="TextInputTagHelper"/>.
        /// </summary>
        public TextInputTagHelper()
            : this(htmlGenerator: null, modelHelper: null)
        {
        }

        internal TextInputTagHelper(IGovUkHtmlGenerator? htmlGenerator = null, IModelHelper? modelHelper = null)
            : base(
                  htmlGenerator ?? new ComponentGenerator(),
                  modelHelper ?? new DefaultModelHelper())
        {
        }

        /// <summary>
        /// The <c>autocomplete</c> attribute for the generated <c>input</c> element.
        /// </summary>
        [HtmlAttributeName(AutocompleteAttributeName)]
        public string? Autocomplete { get; set; }

        /// <summary>
        /// One or more element IDs to add to the <c>aria-describedby</c> attribute of the generated <c>input</c> element.
        /// </summary>
        [HtmlAttributeName(DescribedByAttributeName)]
        public new string? DescribedBy
        {
            get => base.DescribedBy;
            set => base.DescribedBy = value;
        }

        /// <summary>
        /// Whether the <c>disabled</c> attribute should be added to the generated <c>input</c> element.
        /// </summary>
        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; } = ComponentGenerator.InputDefaultDisabled;

        /// <summary>
        /// The <c>id</c> attribute for the generated <c>input</c> element.
        /// </summary>
        /// <remarks>
        /// If not specified then a value is generated from the <c>name</c> attribute.
        /// </remarks>
        [HtmlAttributeName(IdAttributeName)]
        public string? Id { get; set; }

        /// <summary>
        /// Additional attributes to add to the generated <c>input</c> element.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string>? InputAttributes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// The <c>name</c> attribute for the generated <c>input</c> element.
        /// </summary>
        /// <remarks>
        /// Required unless <see cref="FormGroupTagHelperBase.AspFor"/> is specified.
        /// </remarks>
        [HtmlAttributeName(NameAttributeName)]
        public string? Name { get; set; }

        /// <summary>
        /// The <c>inputmode</c> attribute for the generated <c>input</c> element.
        /// </summary>
        [HtmlAttributeName(InputModeAttributeName)]
        public string? InputMode { get; set; }

        /// <summary>
        /// The <c>pattern</c> attribute for the generated <c>input</c> element.
        /// </summary>
        [HtmlAttributeName(PatternAttributeName)]
        public string? Pattern { get; set; }

        /// <summary>
        /// The <c>spellcheck</c> attribute for the generated <c>input</c> element.
        /// </summary>
        [HtmlAttributeName(SpellcheckAttributeName)]
        public bool? Spellcheck { get; set; }

        /// <summary>
        /// The <c>type</c> attribute for the generated <c>input</c> element.
        /// </summary>
        /// <remarks>
        /// The default is <c>&quot;text&quot;</c>.
        /// </remarks>
        [HtmlAttributeName(TypeAttributeName)]
        [DisallowNull]
        public string? Type
        {
            get => _type;
            set => _type = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }

        /// <summary>
        /// The <c>value</c> attribute for the generated <c>input</c> element.
        /// </summary>
        /// <remarks>
        /// If not specified and <see cref="FormGroupTagHelperBase.AspFor"/> is not <c>null</c> then the value
        /// for the specified model expression will be used.
        /// </remarks>
        [HtmlAttributeName(ValueAttributeName)]
        public string? Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueSpecified = true;
            }
        }

        private protected override FormGroupContext CreateFormGroupContext() => new TextInputContext();

        private protected override IHtmlContent GenerateFormGroupContent(
            TagHelperContext context,
            FormGroupContext formGroupContext,
            out bool haveError)
        {
            var inputContext = context.GetContextItem<TextInputContext>();

            var contentBuilder = new HtmlContentBuilder();

            var label = GenerateLabel(formGroupContext);
            contentBuilder.AppendHtml(label);

            var hint = GenerateHint(formGroupContext);
            if (hint != null)
            {
                contentBuilder.AppendHtml(hint);
            }

            var errorMessage = GenerateErrorMessage(formGroupContext);
            if (errorMessage != null)
            {
                contentBuilder.AppendHtml(errorMessage);
            }

            haveError = errorMessage != null;

            var inputTagBuilder = GenerateInput(haveError);
            contentBuilder.AppendHtml(inputTagBuilder);

            return contentBuilder;

            TagBuilder GenerateInput(bool haveError)
            {
                var resolvedId = ResolveIdPrefix();
                var resolvedName = ResolveName();
                var resolvedType = Type ?? ComponentGenerator.InputDefaultType;

                var resolvedValue = _valueSpecified ?
                    Value :
                    AspFor != null ? ModelHelper.GetModelValue(ViewContext, AspFor.ModelExplorer, AspFor.Name) :
                    null;

                return Generator.GenerateTextInput(
                    haveError,
                    resolvedId,
                    resolvedName,
                    resolvedType,
                    resolvedValue,
                    DescribedBy,
                    Autocomplete,
                    Pattern,
                    InputMode,
                    Spellcheck,
                    Disabled,
                    InputAttributes,
                    inputContext.Prefix?.Content,
                    inputContext.Prefix?.Attributes,
                    inputContext.Suffix?.Content,
                    inputContext.Suffix?.Attributes);
            }
        }

        private protected override string ResolveIdPrefix()
        {
            if (Id != null)
            {
                return Id;
            }

            if (Name == null && AspFor == null)
            {
                throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                    IdAttributeName,
                    NameAttributeName,
                    AspForAttributeName);
            }

            var resolvedName = ResolveName();

            return TagBuilder.CreateSanitizedId(resolvedName, Constants.IdAttributeDotReplacement);
        }

        private string ResolveName()
        {
            if (Name == null && AspFor == null)
            {
                throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                    NameAttributeName,
                    AspForAttributeName);
            }

            return Name ?? ModelHelper.GetFullHtmlFieldName(ViewContext, AspFor!.Name);
        }
    }
}
