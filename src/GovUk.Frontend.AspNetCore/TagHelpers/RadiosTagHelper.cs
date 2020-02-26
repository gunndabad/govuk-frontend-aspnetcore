using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-radios", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-radios-divider", "govuk-radios-fieldset", "govuk-radios-item")]
    public class RadiosTagHelper : FormGroupTagHelperBase
    {
        private const string IdPrefixAttributeName = "id-prefix";

        public RadiosTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(IdPrefixAttributeName)]
        public string IdPrefix { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Name == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{NameAttributeName}' and '{AspForAttributeName}' attributes must be specified.");
            }

            var radiosContext = new RadiosContext(ResolvedId, ResolvedName);
            context.Items.Add(RadiosContext.ContextName, radiosContext);

            return base.ProcessAsync(context, output);
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

            foreach (var item in radiosContext.Items)
            {
                contentBuilder.AppendHtml(item);
            }

            var tagBuilder = Generator.GenerateRadios(radiosContext.IsConditional, contentBuilder);

            if (radiosContext.FieldsetFactory != null)
            {
                tagBuilder = radiosContext.FieldsetFactory(Generator, tagBuilder);
            }

            return tagBuilder;
        }

        private protected override string GetIdPrefix() => IdPrefix ?? Name;
    }

    [HtmlTargetElement("govuk-radios-divider", ParentTag = "govuk-radios")]
    public class RadiosDividerTagHelper : TagHelper
    {
        private readonly IGovUkHtmlGenerator _htmlGenerator;

        public RadiosDividerTagHelper(IGovUkHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

            var content = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GenerateRadioItemDivider(content.Snapshot());
            radiosContext.AddItem(tagBuilder);
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

            var contentSnapshot = content.Snapshot();
            radiosContext.SetFieldsetFactory(
                (generator, c) => generator.GenerateFieldset(
                    describedBy: null,
                    IsPageHeading,
                    role: null,
                    contentSnapshot,
                    c));
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
                throw new InvalidOperationException($"The '{ValueAttributeName}' must be specified.");
            }

            var itemContext = new RadiosItemContext();
            context.Items.Add(RadiosItemContext.ContextName, itemContext);

            var radiosContext = (RadiosContext)context.Items[RadiosContext.ContextName];

            var index = radiosContext.Items.Count;
            var resolvedId = Id ?? (index == 0 ? radiosContext.IdPrefix : $"{radiosContext.IdPrefix}-{index}");
            var conditionalId = "conditional-" + resolvedId;
            var hintId = resolvedId + "-item-hint";

            var childContent = await output.GetChildContentAsync();

            var tagBuilder = _htmlGenerator.GenerateRadioItem(
                resolvedId,
                radiosContext.ResolvedName,
                Value,
                Checked,
                Disabled,
                childContent,
                conditionalId,
                itemContext.ConditionalContent,
                hintId,
                itemContext.HintContent);

            radiosContext.AddItem(tagBuilder);

            if (itemContext.ConditionalContent != null)
            {
                radiosContext.IsConditional = true;
            }

            output.SuppressOutput();
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

    internal class RadiosContext
    {
        public const string ContextName = nameof(RadiosContext);

        private readonly List<IHtmlContent> _items;

        public RadiosContext(string idPrefix, string resolvedName)
        {
            IdPrefix = idPrefix ?? throw new ArgumentNullException(nameof(idPrefix));
            ResolvedName = resolvedName ?? throw new ArgumentNullException(nameof(resolvedName));
            _items = new List<IHtmlContent>();
        }

        public delegate TagBuilder CreateFieldset(IGovUkHtmlGenerator generator, IHtmlContent content);

        public CreateFieldset FieldsetFactory { get; private set; }
        public string IdPrefix { get; }
        public bool IsConditional { get; set; }
        public IReadOnlyCollection<IHtmlContent> Items => _items;
        public string ResolvedName { get; }

        public void AddItem(IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            _items.Add(content);
        }

        public void SetFieldsetFactory(CreateFieldset factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (FieldsetFactory != null)
            {
                throw new InvalidOperationException("Fieldset has already been specified.");
            }

            FieldsetFactory = factory;
        }
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
}
