using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-checkboxes", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-checkboxes-fieldset", "govuk-checkboxes-item", "govuk-checkboxes-hint", "govuk-checkboxes-error-message")]
    public class CheckboxesTagHelper : FormGroupTagHelperBase
    {
        private const string AttributesPrefix = "checkboxes-";
        private const string IdPrefixAttributeName = "id-prefix";

        public CheckboxesTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        [HtmlAttributeName(IdPrefixAttributeName)]
        public string IdPrefix { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            if (Name == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{NameAttributeName}' and '{AspForAttributeName}' attributes must be specified.");
            }

            var checkboxesContext = new CheckboxesContext(ResolvedId, ResolvedName);

            using (context.SetScopedContextItem(CheckboxesContext.ContextName, checkboxesContext))
            {
                await base.ProcessAsync(context, output);
            }
        }

        private protected override TagBuilder GenerateContent(TagHelperContext context, FormGroupBuilder builder)
        {
            var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

            var contentBuilder = new HtmlContentBuilder();

            var hint = GenerateHint(builder);
            if (hint != null)
            {
                contentBuilder.AppendHtml(hint);
            }

            var errorMessage = GenerateErrorMessage(builder);
            if (errorMessage != null)
            {
                contentBuilder.AppendHtml(errorMessage);
            }

            var haveError = errorMessage != null;

            var resolvedDescribedBy = checkboxesContext.Fieldset?.DescribedBy ?? DescribedBy;

            var tagBuilder = Generator.GenerateCheckboxes(
                ResolvedName,
                checkboxesContext.IsConditional,
                resolvedDescribedBy,
                Attributes,
                checkboxesContext.Items);
            contentBuilder.AppendHtml(tagBuilder);

            IHtmlContent content = contentBuilder;
            if (checkboxesContext.Fieldset != null)
            {
                content = Generator.GenerateFieldset(
                    DescribedBy,
                    checkboxesContext.Fieldset.IsPageHeading,
                    role: null,
                    attributes: Attributes,
                    legendContent: checkboxesContext.Fieldset.LegendContent,
                    legendAttributes: checkboxesContext.Fieldset.LegendAttributes,
                    content: content);
            }

            return Generator.GenerateFormGroup(haveError, FormGroupAttributes, content);
        }

        private protected override string GetIdPrefix() => IdPrefix ?? Name;
    }

    [HtmlTargetElement("govuk-checkboxes-fieldset", ParentTag = "govuk-checkboxes", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-checkboxes-fieldset-legend")]
    public class CheckboxesFieldsetTagHelper : TagHelper
    {
        private const string AttributesPrefix = "fieldset-";
        private const string DescribedByAttributeName = "described-by";
        private const string IsPageHeadingAttributeName = "is-page-heading";

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        [HtmlAttributeName(DescribedByAttributeName)]
        public string DescribedBy { get; set; }

        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

            var fieldsetContext = new CheckboxesFieldsetContext();

            using (context.SetScopedContextItem(CheckboxesFieldsetContext.ContextName, fieldsetContext))
            {
                await output.GetChildContentAsync();
            }

            checkboxesContext.SetFieldset(new CheckboxesFieldset()
            {
                Attributes = Attributes,
                DescribedBy = DescribedBy,
                IsPageHeading = IsPageHeading,
                LegendContent = fieldsetContext.Legend?.content,
                LegendAttributes = fieldsetContext.Legend?.attributes
            });

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-checkboxes-fieldset-legend", ParentTag = "govuk-checkboxes-fieldset")]
    public class CheckboxesFieldsetLegendTagHelper : TagHelper
    {
        private const string AttributesPrefix = "legend-*";

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var fieldsetContext = (CheckboxesFieldsetContext)context.Items[CheckboxesFieldsetContext.ContextName];

            var childContent = await output.GetChildContentAsync();

            if (!fieldsetContext.TrySetLegend(Attributes, childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here");
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-checkboxes-item", ParentTag = "govuk-checkboxes")]
    public class CheckboxesItemTagHelper : TagHelper
    {
        private const string AttributesPrefix = "checkboxes-item-";
        private const string CheckedAttributeName = "checked";
        private const string DisabledAttributeName = "disabled";
        private const string IdAttributeName = "id";
        private const string ValueAttributeName = "value";

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        [HtmlAttributeName(CheckedAttributeName)]
        public bool Checked { get; set; }

        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(ValueAttributeName)]
        public string Value { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            if (Value == null)
            {
                throw new InvalidOperationException($"The '{ValueAttributeName}' attribute must be specified.");
            }

            var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

            var index = checkboxesContext.Items.Count;
            var resolvedId = Id ?? (index == 0 ? checkboxesContext.IdPrefix : $"{checkboxesContext.IdPrefix}-{index}");
            var conditionalId = "conditional-" + resolvedId;
            var hintId = resolvedId + "-item-hint";

            var itemContext = new CheckboxesItemContext();

            TagHelperContent childContent;
            using (context.SetScopedContextItem(CheckboxesItemContext.ContextName, itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            checkboxesContext.AddItem(new CheckboxesItem()
            {
                Attributes = Attributes,
                Checked = Checked,
                ConditionalContent = itemContext.ConditionalContent,
                ConditionalId = conditionalId,
                Content = childContent.Snapshot(),
                Disabled = Disabled,
                HintAttributes = itemContext.Hint?.attributes,
                HintContent = itemContext.Hint?.content,
                HintId = hintId,
                Id = resolvedId,
                Name = checkboxesContext.ResolvedName,
                Value = Value
            });

            if (itemContext.ConditionalContent != null)
            {
                checkboxesContext.SetIsConditional();
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-checkboxes-item-conditional", ParentTag = "govuk-checkboxes-item")]
    public class CheckboxesItemConditionalTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var itemContext = (CheckboxesItemContext)context.Items[CheckboxesItemContext.ContextName];

            var content = await output.GetChildContentAsync();

            itemContext.SetConditionalContent(content.Snapshot());

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-checkboxes-item-hint", ParentTag = "govuk-checkboxes-item")]
    public class CheckboxesItemHintTagHelper : TagHelper
    {
        private const string AttributesPrefix = "hint-";

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var itemContext = (CheckboxesItemContext)context.Items[CheckboxesItemContext.ContextName];

            var content = await output.GetChildContentAsync();

            itemContext.SetHint(Attributes, content.Snapshot());

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-checkboxes-hint", ParentTag = "govuk-checkboxes")]
    public class CheckboxesHintTagHelper : FormGroupHintTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-checkboxes-error-message", ParentTag = "govuk-checkboxes")]
    public class CheckboxesErrorMessageTagHelper : FormGroupErrorMessageTagHelperBase
    {
    }

    internal class CheckboxesContext
    {
        public const string ContextName = nameof(CheckboxesContext);

        private readonly List<CheckboxesItem> _items;

        public CheckboxesContext(string idPrefix, string resolvedName)
        {
            IdPrefix = idPrefix ?? throw new ArgumentNullException(nameof(idPrefix));
            ResolvedName = resolvedName ?? throw new ArgumentNullException(nameof(resolvedName));
            _items = new List<CheckboxesItem>();
        }

        public CheckboxesFieldset Fieldset { get; private set; }
        public string IdPrefix { get; }
        public bool IsConditional { get; private set; }
        public IReadOnlyCollection<CheckboxesItem> Items => _items;
        public string ResolvedName { get; }

        public void AddItem(CheckboxesItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }

        public void SetFieldset(CheckboxesFieldset fieldset)
        {
            if (fieldset == null)
            {
                throw new ArgumentNullException(nameof(fieldset));
            }

            if (Fieldset != null)
            {
                throw new InvalidOperationException("Fieldset has already been specified.");
            }

            Fieldset = fieldset;
        }

        public void SetIsConditional() => IsConditional = true;
    }

    internal class CheckboxesFieldsetContext
    {
        public const string ContextName = nameof(CheckboxesFieldsetContext);

        public (IDictionary<string, string> attributes, IHtmlContent content)? Legend { get; private set; }

        public bool TrySetLegend(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Legend != null)
            {
                return false;
            }

            Legend = (attributes, content);
            return true;
        }
    }

    internal class CheckboxesItemContext
    {
        public const string ContextName = nameof(CheckboxesItemContext);

        public IHtmlContent ConditionalContent { get; private set; }
        public (IDictionary<string, string> attributes, IHtmlContent content)? Hint { get; private set; }

        public void SetConditionalContent(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (ConditionalContent != null)
            {
                throw new InvalidOperationException("Conditional content has already been set.");
            }

            ConditionalContent = content;
        }

        public void SetHint(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Hint != null)
            {
                throw new InvalidOperationException("Hint content has already been set.");
            }

            Hint = (attributes, content);
        }
    }

    internal class CheckboxesFieldset
    {
        public bool IsPageHeading { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public IHtmlContent LegendContent { get; set; }
        public IDictionary<string, string> LegendAttributes { get; set; }
        public string DescribedBy { get; set; }
    }
}
