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
        private const string CheckboxesAttributesPrefix = "checkboxes-";
        private const string IdPrefixAttributeName = "id-prefix";

        public CheckboxesTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(DictionaryAttributePrefix = CheckboxesAttributesPrefix)]
        public IDictionary<string, string> CheckboxesAttributes { get; set; } = new Dictionary<string, string>();

        [HtmlAttributeName(IdPrefixAttributeName)]
        public string IdPrefix { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Name == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{NameAttributeName}' and '{AspForAttributeName}' attributes must be specified.");
            }

            var checkboxesContext = new CheckboxesContext(ResolvedId, ResolvedName);
            using (context.SetScopedContextItem(typeof(CheckboxesContext), checkboxesContext))
            {
                await base.ProcessAsync(context, output);
            }
        }

        protected override TagBuilder GenerateContent(TagHelperContext context, FormGroupBuilder builder)
        {
            var checkboxesContext = (CheckboxesContext)context.Items[typeof(CheckboxesContext)];

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
                CheckboxesAttributes,
                checkboxesContext.Items);
            contentBuilder.AppendHtml(tagBuilder);

            IHtmlContent content = contentBuilder;
            if (checkboxesContext.Fieldset != null)
            {
                content = Generator.GenerateFieldset(
                    DescribedBy,
                    checkboxesContext.Fieldset.IsPageHeading,
                    role: null,
                    legendContent: checkboxesContext.Fieldset.LegendContent,
                    legendAttributes: checkboxesContext.Fieldset.LegendAttributes,
                    content: content,
                    attributes: checkboxesContext.Fieldset.Attributes);
            }

            return Generator.GenerateFormGroup(haveError, content, FormGroupAttributes);
        }

        protected override string GetIdPrefix() => IdPrefix ?? Name;
    }

    [HtmlTargetElement("govuk-checkboxes-fieldset", ParentTag = "govuk-checkboxes", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-checkboxes-fieldset-legend")]
    public class CheckboxesFieldsetTagHelper : TagHelper
    {
        private const string DescribedByAttributeName = "described-by";
        private const string IsPageHeadingAttributeName = "is-page-heading";

        [HtmlAttributeName(DescribedByAttributeName)]
        public string DescribedBy { get; set; }

        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var checkboxesContext = (CheckboxesContext)context.Items[typeof(CheckboxesContext)];

            var fieldsetContext = new CheckboxesFieldsetContext();

            using (context.SetScopedContextItem(typeof(CheckboxesFieldsetContext), fieldsetContext))
            {
                await output.GetChildContentAsync();
            }

            checkboxesContext.SetFieldset(new CheckboxesFieldset()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
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
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldsetContext = (CheckboxesFieldsetContext)context.Items[typeof(CheckboxesFieldsetContext)];

            var childContent = await output.GetChildContentAsync();

            if (!fieldsetContext.TrySetLegend(output.Attributes.ToAttributesDictionary(), childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here");
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-checkboxes-item", ParentTag = "govuk-checkboxes")]
    public class CheckboxesItemTagHelper : TagHelper
    {
        private const string CheckedAttributeName = "checked";
        private const string DisabledAttributeName = "disabled";
        private const string IdAttributeName = "id";
        private const string InputAttributesPrefix = "input-";
        private const string ValueAttributeName = "value";

        [HtmlAttributeName(CheckedAttributeName)]
        public bool Checked { get; set; }

        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = InputAttributesPrefix)]
        public IDictionary<string, string> InputAttributes { get; set; } = new Dictionary<string, string>();

        [HtmlAttributeName(ValueAttributeName)]
        public string Value { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Value == null)
            {
                throw new InvalidOperationException($"The '{ValueAttributeName}' attribute must be specified.");
            }

            var checkboxesContext = (CheckboxesContext)context.Items[typeof(CheckboxesContext)];

            var index = checkboxesContext.Items.Count;
            var resolvedId = Id ?? (index == 0 ? checkboxesContext.IdPrefix : $"{checkboxesContext.IdPrefix}-{index}");
            var conditionalId = "conditional-" + resolvedId;
            var hintId = resolvedId + "-item-hint";

            var itemContext = new CheckboxesItemContext();

            TagHelperContent childContent;
            using (context.SetScopedContextItem(typeof(CheckboxesItemContext), itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            checkboxesContext.AddItem(new CheckboxesItem()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
                Checked = Checked,
                ConditionalContent = itemContext.Conditional?.content,
                ConditionalAttributes = itemContext.Conditional?.attributes,
                ConditionalId = conditionalId,
                Content = childContent.Snapshot(),
                Disabled = Disabled,
                HintAttributes = itemContext.Hint?.attributes,
                HintContent = itemContext.Hint?.content,
                HintId = hintId,
                Id = resolvedId,
                InputAttributes = InputAttributes,
                Name = checkboxesContext.ResolvedName,
                Value = Value
            });

            if (itemContext.Conditional != null)
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
            var itemContext = (CheckboxesItemContext)context.Items[typeof(CheckboxesItemContext)];

            var content = await output.GetChildContentAsync();

            itemContext.SetConditional(output.Attributes.ToAttributesDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-checkboxes-item-hint", ParentTag = "govuk-checkboxes-item")]
    public class CheckboxesItemHintTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = (CheckboxesItemContext)context.Items[typeof(CheckboxesItemContext)];

            var content = await output.GetChildContentAsync();

            itemContext.SetHint(output.Attributes.ToAttributesDictionary(), content.Snapshot());

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
        public (IDictionary<string, string> attributes, IHtmlContent content)? Conditional { get; private set; }
        public (IDictionary<string, string> attributes, IHtmlContent content)? Hint { get; private set; }

        public void SetConditional(IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Conditional != null)
            {
                throw new InvalidOperationException("Conditional content has already been set.");
            }

            Conditional = (attributes, content);
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
