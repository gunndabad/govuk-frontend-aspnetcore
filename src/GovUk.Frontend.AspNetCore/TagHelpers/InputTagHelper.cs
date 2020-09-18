﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-input")]
    [RestrictChildren("govuk-input-label", "govuk-input-hint", "govuk-input-error-message")]
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
        public string Type { get; set; }

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
                Attributes);
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
}
