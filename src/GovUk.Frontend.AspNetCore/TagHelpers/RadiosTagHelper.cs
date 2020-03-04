using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-radios", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-radios-divider", "govuk-radios-fieldset", "govuk-radios-item", "govuk-radios-hint", "govuk-radios-error-message")]
    public class RadiosTagHelper : FormGroupTagHelperBase
    {
        private const string IdPrefixAttributeName = "id-prefix";

        public RadiosTagHelper(IGovUkHtmlGenerator htmlGenerator)
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

            var radiosContext = new RadiosContext(ResolvedId, ResolvedName);
            using (context.SetScopedContextItem(RadiosContext.ContextName, radiosContext))
            {
                await base.ProcessAsync(context, output);
            }
        }

        private protected override TagBuilder GenerateContent(TagHelperContext context, FormGroupBuilder builder)
        {
            var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

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

            var tagBuilder = Generator.GenerateRadios(ResolvedName, radiosContext.IsConditional, radiosContext.Items);
            contentBuilder.AppendHtml(tagBuilder);

            IHtmlContent content = contentBuilder;
            if (radiosContext.Fieldset != null)
            {
                content = Generator.GenerateFieldset(
                    DescribedBy,
                    radiosContext.Fieldset.IsPageHeading,
                    role: null,
                    legendContent: radiosContext.Fieldset.LegendContent,
                    content: content);
            }

            return Generator.GenerateFormGroup(haveError, content);
        }

        private protected override string GetIdPrefix() => IdPrefix ?? Name;
    }

    [HtmlTargetElement("govuk-radios-divider", ParentTag = "govuk-radios")]
    public class RadiosDividerTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

            var content = await output.GetChildContentAsync();

            radiosContext.AddItem(new RadiosItemDivider()
            {
                Content = content.Snapshot()
            });
        }
    }

    [HtmlTargetElement("govuk-radios-fieldset", ParentTag = "govuk-radios", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RadiosFieldsetTagHelper : TagHelper
    {
        private const string IsPageHeadingAttributeName = "is-page-heading";

        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

            var content = await output.GetChildContentAsync();

            radiosContext.SetFieldset(new RadiosFieldset()
            {
                IsPageHeading = IsPageHeading,
                LegendContent = content.Snapshot()
            });
        }
    }

    [HtmlTargetElement("govuk-radios-item", ParentTag = "govuk-radios")]
    public class RadiosItemTagHelper : TagHelper
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

            var itemContext = new RadiosItemContext();
            using (context.SetScopedContextItem(RadiosItemContext.ContextName, itemContext))
            {
                var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

                var index = radiosContext.Items.Count;
                var resolvedId = Id ?? (index == 0 ? radiosContext.IdPrefix : $"{radiosContext.IdPrefix}-{index}");
                var conditionalId = "conditional-" + resolvedId;
                var hintId = resolvedId + "-item-hint";

                var childContent = await output.GetChildContentAsync();

                radiosContext.AddItem(new RadiosItem()
                {
                    Checked = Checked,
                    ConditionalContent = itemContext.ConditionalContent,
                    ConditionalId = conditionalId,
                    Content = childContent.Snapshot(),
                    Disabled = Disabled,
                    HintContent = itemContext.HintContent,
                    HintId = hintId,
                    Id = resolvedId,
                    Value = Value
                });

                if (itemContext.ConditionalContent != null)
                {
                    radiosContext.SetIsConditional();
                }

                output.SuppressOutput();
            }
        }
    }

    [HtmlTargetElement("govuk-radios-item-conditional", ParentTag = "govuk-radios-item")]
    public class RadiosItemConditionalTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = (RadiosItemContext)context.Items[RadiosItemContext.ContextName];

            var content = await output.GetChildContentAsync();

            itemContext.SetConditionalContent(content.Snapshot());
        }
    }

    [HtmlTargetElement("govuk-radios-item-hint", ParentTag = "govuk-radios-item")]
    public class RadiosItemHintTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = (RadiosItemContext)context.Items[RadiosItemContext.ContextName];

            var content = await output.GetChildContentAsync();

            itemContext.SetHintContent(content.Snapshot());
        }
    }

    [HtmlTargetElement("govuk-radios-hint", ParentTag = "govuk-radios")]
    public class RadiosHintTagHelper : FormGroupHintTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-radios-error-message", ParentTag = "govuk-radios")]
    public class RadiosErrorMessageTagHelper : FormGroupErrorMessageTagHelperBase
    {
    }

    internal class RadiosContext
    {
        public const string ContextName = nameof(RadiosContext);

        private readonly List<RadiosItemBase> _items;

        public RadiosContext(string idPrefix, string resolvedName)
        {
            IdPrefix = idPrefix ?? throw new ArgumentNullException(nameof(idPrefix));
            ResolvedName = resolvedName ?? throw new ArgumentNullException(nameof(resolvedName));
            _items = new List<RadiosItemBase>();
        }

        public RadiosFieldset Fieldset { get; private set; }
        public string IdPrefix { get; }
        public bool IsConditional { get; private set; }
        public IReadOnlyCollection<RadiosItemBase> Items => _items;
        public string ResolvedName { get; }

        public void AddItem(RadiosItemBase item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }

        public void SetFieldset(RadiosFieldset fieldset)
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

    internal class RadiosItemContext
    {
        public const string ContextName = nameof(RadiosItemContext);

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

    internal class RadiosFieldset
    {
        public bool IsPageHeading { get; set; }
        public IHtmlContent LegendContent { get; set; }
    }
}
