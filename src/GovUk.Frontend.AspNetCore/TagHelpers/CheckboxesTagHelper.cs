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
        private const string IdPrefixAttributeName = "id-prefix";

        public CheckboxesTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

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
                    content: content);
            }

            return Generator.GenerateFormGroup(haveError, content);
        }

        private protected override string GetIdPrefix() => IdPrefix ?? Name;
    }

    [HtmlTargetElement("govuk-checkboxes-fieldset", ParentTag = "govuk-checkboxes", TagStructure = TagStructure.NormalOrSelfClosing)]
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
            var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

            var content = await output.GetChildContentAsync();

            checkboxesContext.SetFieldset(new CheckboxesFieldset()
            {
                DescribedBy = DescribedBy,
                IsPageHeading = IsPageHeading,
                LegendContent = content.Snapshot()
            });

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-checkboxes-item", ParentTag = "govuk-checkboxes")]
    public class CheckboxesItemTagHelper : TagHelper
    {
        private const string CheckedAttributeName = "checked";
        private const string DisabledAttributeName = "disabled";
        private const string IdAttributeName = "id";
        private const string ValueAttributeName = "value";

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
            if (Value == null)
            {
                throw new InvalidOperationException($"The '{ValueAttributeName}' attribute must be specified.");
            }

            var itemContext = new CheckboxesItemContext();
            using (context.SetScopedContextItem(CheckboxesItemContext.ContextName, itemContext))
            {
                var checkboxesContext = (CheckboxesContext)context.Items[CheckboxesContext.ContextName];

                var index = checkboxesContext.Items.Count;
                var resolvedId = Id ?? (index == 0 ? checkboxesContext.IdPrefix : $"{checkboxesContext.IdPrefix}-{index}");
                var conditionalId = "conditional-" + resolvedId;
                var hintId = resolvedId + "-item-hint";

                var childContent = await output.GetChildContentAsync();

                checkboxesContext.AddItem(new CheckboxesItem()
                {
                    Checked = Checked,
                    ConditionalContent = itemContext.ConditionalContent,
                    ConditionalId = conditionalId,
                    Content = childContent.Snapshot(),
                    Disabled = Disabled,
                    HintContent = itemContext.HintContent,
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
    }

    [HtmlTargetElement("govuk-checkboxes-item-conditional", ParentTag = "govuk-checkboxes-item")]
    public class CheckboxesItemConditionalTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = (CheckboxesItemContext)context.Items[CheckboxesItemContext.ContextName];

            var content = await output.GetChildContentAsync();

            itemContext.SetConditionalContent(content.Snapshot());

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-checkboxes-item-hint", ParentTag = "govuk-checkboxes-item")]
    public class CheckboxesItemHintTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = (CheckboxesItemContext)context.Items[CheckboxesItemContext.ContextName];

            var content = await output.GetChildContentAsync();

            itemContext.SetHintContent(content.Snapshot());

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

    internal class CheckboxesItemContext
    {
        public const string ContextName = nameof(CheckboxesItemContext);

        public IHtmlContent ConditionalContent { get; private set; }
        public IHtmlContent HintContent { get; private set; }

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

        public void SetHintContent(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (HintContent != null)
            {
                throw new InvalidOperationException("Hint content has already been set.");
            }

            HintContent = content;
        }
    }

    internal class CheckboxesFieldset
    {
        public bool IsPageHeading { get; set; }
        public IHtmlContent LegendContent { get; set; }
        public string DescribedBy { get; set; }
    }
}
