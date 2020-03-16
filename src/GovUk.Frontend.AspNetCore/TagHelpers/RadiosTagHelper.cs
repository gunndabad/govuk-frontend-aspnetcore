using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-radios", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-radios-divider", "govuk-radios-fieldset", "govuk-radios-item", "govuk-radios-hint", "govuk-radios-error-message")]
    public class RadiosTagHelper : FormGroupTagHelperBase
    {
        private const string AttributesPrefix = "radios-";
        private const string IdPrefixAttributeName = "id-prefix";

        public RadiosTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        [HtmlAttributeName(IdPrefixAttributeName)]
        public string IdPrefix { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Name == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{NameAttributeName}' and '{AspForAttributeName}' attributes must be specified.");
            }

            var radiosContext = new RadiosContext(ResolvedId, ResolvedName, ViewContext, AspFor);
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
                    attributes: Attributes,
                    legendContent: radiosContext.Fieldset.LegendContent,
                    legendAttributes: radiosContext.Fieldset.LegendAttributes,
                    content: content);
            }

            return Generator.GenerateFormGroup(haveError, FormGroupAttributes, content);
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

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-radios-fieldset", ParentTag = "govuk-radios", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-radios-fieldset-legend")]
    public class RadiosFieldsetTagHelper : TagHelper
    {
        private const string AttributesPrefix = "fieldset-";
        private const string IsPageHeadingAttributeName = "is-page-heading";

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

            var fieldsetContext = new RadiosFieldsetContext();

            using (context.SetScopedContextItem(RadiosFieldsetContext.ContextName, fieldsetContext))
            {
                await output.GetChildContentAsync();
            }

            radiosContext.SetFieldset(new RadiosFieldset()
            {
                Attributes = Attributes,
                IsPageHeading = IsPageHeading,
                LegendContent = fieldsetContext.Legend?.content,
                LegendAttributes = fieldsetContext.Legend?.attributes
            });

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-radios-fieldset-legend", ParentTag = "govuk-radios-fieldset")]
    public class RadiosFieldsetLegendTagHelper : TagHelper
    {
        private const string AttributesPrefix = "legend-";

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var fieldsetContext = (RadiosFieldsetContext)context.Items[RadiosFieldsetContext.ContextName];

            var childContent = await output.GetChildContentAsync();

            if (!fieldsetContext.TrySetLegend(Attributes, childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here");
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-radios-item", ParentTag = "govuk-radios")]
    public class RadiosItemTagHelper : TagHelper
    {
        private const string CheckedAttributeName = "checked";
        private const string DisabledAttributeName = "disabled";
        private const string IdAttributeName = "id";
        private const string ValueAttributeName = "value";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public RadiosItemTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        [HtmlAttributeName(CheckedAttributeName)]
        public bool? Checked { get; set; }

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

            // REVIEW Consider throwing if Checked is null && there is no AspFor

            var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

            var index = radiosContext.Items.Count;
            var resolvedId = Id ?? (index == 0 ? radiosContext.IdPrefix : $"{radiosContext.IdPrefix}-{index}");
            var conditionalId = "conditional-" + resolvedId;
            var hintId = resolvedId + "-item-hint";

            var resolvedChecked = Checked ??
                (radiosContext.HaveModelExpression ?
                    _htmlGenerator.GetModelValue(
                        radiosContext.ViewContext,
                        radiosContext.AspFor.ModelExplorer,
                        radiosContext.AspFor.Name) == Value :
                 false);

            var itemContext = new RadiosItemContext();

            TagHelperContent childContent;
            using (context.SetScopedContextItem(RadiosItemContext.ContextName, itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            radiosContext.AddItem(new RadiosItem()
            {
                Checked = resolvedChecked,
                ConditionalContent = itemContext.ConditionalContent,
                ConditionalId = conditionalId,
                Content = childContent.Snapshot(),
                Disabled = Disabled,
                HintAttributes = itemContext.Hint?.attributes,
                HintContent = itemContext.Hint?.content,
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

    [HtmlTargetElement("govuk-radios-item-conditional", ParentTag = "govuk-radios-item")]
    public class RadiosItemConditionalTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var itemContext = (RadiosItemContext)context.Items[RadiosItemContext.ContextName];

            var content = await output.GetChildContentAsync();

            itemContext.SetConditionalContent(content.Snapshot());

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-radios-item-hint", ParentTag = "govuk-radios-item")]
    public class RadiosItemHintTagHelper : TagHelper
    {
        private const string AttributesPrefix = "hint-";

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.ThrowIfOutputHasAttributes();

            var itemContext = (RadiosItemContext)context.Items[RadiosItemContext.ContextName];

            var content = await output.GetChildContentAsync();

            itemContext.SetHint(Attributes, content.Snapshot());

            output.SuppressOutput();
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

        public RadiosContext(
            string idPrefix,
            string resolvedName,
            ViewContext viewContext,
            ModelExpression @for)
        {
            IdPrefix = idPrefix ?? throw new ArgumentNullException(nameof(idPrefix));
            ResolvedName = resolvedName ?? throw new ArgumentNullException(nameof(resolvedName));
            ViewContext = viewContext;
            AspFor = @for;
            _items = new List<RadiosItemBase>();
        }

        public RadiosFieldset Fieldset { get; private set; }
        public string IdPrefix { get; }
        public bool IsConditional { get; private set; }
        public IReadOnlyCollection<RadiosItemBase> Items => _items;
        public string ResolvedName { get; }
        public ViewContext ViewContext { get; }
        public ModelExpression AspFor { get; }

        public bool HaveModelExpression => AspFor != null;
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

    internal class RadiosFieldsetContext
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

    internal class RadiosItemContext
    {
        public const string ContextName = nameof(RadiosItemContext);

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

    internal class RadiosFieldset
    {
        public bool IsPageHeading { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public IHtmlContent LegendContent { get; set; }
        public IDictionary<string, string> LegendAttributes { get; set; }
    }
}
