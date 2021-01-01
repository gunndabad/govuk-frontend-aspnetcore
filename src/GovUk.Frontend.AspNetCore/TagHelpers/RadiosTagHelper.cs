using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
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
        private const string IdPrefixAttributeName = "id-prefix";
        private const string RadiosAttributesPrefix = "radios-";

        public RadiosTagHelper(IGovUkHtmlGenerator htmlGenerator, IModelHelper modelHelper)
            : base(htmlGenerator, modelHelper)
        {
        }

        [HtmlAttributeName(IdPrefixAttributeName)]
        public string IdPrefix { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = RadiosAttributesPrefix)]
        public IDictionary<string, string> RadiosAttributes { get; set; } = new Dictionary<string, string>();

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Name == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{NameAttributeName}' and '{AspForAttributeName}' attributes must be specified.");
            }

            var radiosContext = new RadiosContext(ResolvedId, ResolvedName, ViewContext, AspFor);
            using (context.SetScopedContextItem(typeof(RadiosContext), radiosContext))
            {
                await base.ProcessAsync(context, output);
            }
        }

        protected override TagBuilder GenerateContent(TagHelperContext context, FormGroupBuilder builder)
        {
            var radiosContext = (RadiosContext)context.Items[typeof(RadiosContext)];

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

            var tagBuilder = Generator.GenerateRadios(
                ResolvedName,
                radiosContext.IsConditional,
                radiosContext.Items,
                RadiosAttributes);
            contentBuilder.AppendHtml(tagBuilder);

            IHtmlContent content = contentBuilder;
            if (radiosContext.Fieldset != null)
            {
                content = Generator.GenerateFieldset(
                    DescribedBy,
                    role: null,
                    radiosContext.Fieldset.LegendIsPageHeading,
                    legendContent: radiosContext.Fieldset.LegendContent,
                    legendAttributes: radiosContext.Fieldset.LegendAttributes,
                    content: content,
                    attributes: radiosContext.Fieldset.Attributes);
            }

            return Generator.GenerateFormGroup(haveError, content, FormGroupAttributes);
        }

        protected override string GetIdPrefix() => IdPrefix ?? Name;
    }

    [HtmlTargetElement("govuk-radios-divider", ParentTag = "govuk-radios")]
    public class RadiosDividerTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var radiosContext = (RadiosContext)context.Items[typeof(RadiosContext)];

            var content = await output.GetChildContentAsync();

            radiosContext.AddItem(new RadiosItemDivider()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
                Content = content.Snapshot()
            });

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-radios-fieldset", ParentTag = "govuk-radios", TagStructure = TagStructure.NormalOrSelfClosing)]
    [RestrictChildren("govuk-radios-fieldset-legend")]
    public class RadiosFieldsetTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var radiosContext = (RadiosContext)context.Items[typeof(RadiosContext)];

            var fieldsetContext = new RadiosFieldsetContext();

            using (context.SetScopedContextItem(typeof(RadiosFieldsetContext), fieldsetContext))
            {
                await output.GetChildContentAsync();
            }

            radiosContext.SetFieldset(new RadiosFieldset()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
                LegendIsPageHeading = fieldsetContext.Legend?.isPageHeading,
                LegendContent = fieldsetContext.Legend?.content,
                LegendAttributes = fieldsetContext.Legend?.attributes
            });

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-radios-fieldset-legend", ParentTag = "govuk-radios-fieldset")]
    public class RadiosFieldsetLegendTagHelper : TagHelper
    {
        private const string IsPageHeadingAttributeName = "is-page-heading";

        [HtmlAttributeName(IsPageHeadingAttributeName)]
        public bool IsPageHeading { get; set; } = ComponentGenerator.FieldsetLegendDefaultIsPageHeading;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldsetContext = (RadiosFieldsetContext)context.Items[typeof(RadiosFieldsetContext)];

            var childContent = await output.GetChildContentAsync();

            if (!fieldsetContext.TrySetLegend(
                IsPageHeading,
                output.Attributes.ToAttributesDictionary(),
                childContent.Snapshot()))
            {
                throw new InvalidOperationException($"Cannot render <{output.TagName}> here");
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-radios-item", ParentTag = "govuk-radios")]
    public class RadiosItemTagHelper : TagHelper
    {
        private const string IdAttributeName = "id";
        private const string InputAttributesPrefix = "input-";
        private const string IsCheckedAttributeName = "checked";
        private const string IsDisabledAttributeName = "disabled";
        private const string ValueAttributeName = "value";

        private readonly IGovUkHtmlGenerator _htmlGenerator;
        private readonly IModelHelper _modelHelper;

        public RadiosItemTagHelper(IGovUkHtmlGenerator htmlGenerator, IModelHelper modelHelper)
        {
            _htmlGenerator = htmlGenerator ?? throw new ArgumentNullException(nameof(htmlGenerator));
            _modelHelper = modelHelper ?? throw new ArgumentNullException(nameof(modelHelper));
        }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = InputAttributesPrefix)]
        public IDictionary<string, string> InputAttributes { get; set; } = new Dictionary<string, string>();

        [HtmlAttributeName(IsCheckedAttributeName)]
        public bool? IsChecked { get; set; }

        [HtmlAttributeName(IsDisabledAttributeName)]
        public bool IsDisabled { get; set; }

        [HtmlAttributeName(ValueAttributeName)]
        public string Value { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Value == null)
            {
                throw new InvalidOperationException($"The '{ValueAttributeName}' attribute must be specified.");
            }

            // REVIEW Consider throwing if Checked is null && there is no AspFor

            var radiosContext = (RadiosContext)context.Items[typeof(RadiosContext)];

            var index = radiosContext.Items.Count;
            var resolvedId = Id ?? (index == 0 ? radiosContext.IdPrefix : $"{radiosContext.IdPrefix}-{index}");
            var conditionalId = "conditional-" + resolvedId;
            var hintId = resolvedId + "-item-hint";

            var resolvedChecked = IsChecked ??
                (radiosContext.HaveModelExpression ?
                    _modelHelper.GetModelValue(
                        radiosContext.ViewContext,
                        radiosContext.AspFor.ModelExplorer,
                        radiosContext.AspFor.Name) == Value :
                 false);

            var itemContext = new RadiosItemContext();

            TagHelperContent childContent;
            using (context.SetScopedContextItem(typeof(RadiosItemContext), itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            radiosContext.AddItem(new RadiosItem()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
                IsChecked = resolvedChecked,
                ConditionalContent = itemContext.Conditional?.content,
                ConditionalAttributes = itemContext.Conditional?.attributes,
                ConditionalId = conditionalId,
                Content = childContent.Snapshot(),
                IsDisabled = IsDisabled,
                HintAttributes = itemContext.Hint?.attributes,
                HintContent = itemContext.Hint?.content,
                HintId = hintId,
                Id = resolvedId,
                InputAttributes = InputAttributes,
                Value = Value
            });

            if (itemContext.Conditional != null)
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
            var itemContext = (RadiosItemContext)context.Items[typeof(RadiosItemContext)];

            var content = await output.GetChildContentAsync();

            itemContext.SetConditional(output.Attributes.ToAttributesDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("govuk-radios-item-hint", ParentTag = "govuk-radios-item")]
    public class RadiosItemHintTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var itemContext = (RadiosItemContext)context.Items[typeof(RadiosItemContext)];

            var content = await output.GetChildContentAsync();

            itemContext.SetHint(output.Attributes.ToAttributesDictionary(), content.Snapshot());

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
        private readonly List<RadiosItemBase> _items;

        public RadiosContext(
            string idPrefix,
            string resolvedName,
            ViewContext viewContext,
            ModelExpression aspFor)
        {
            IdPrefix = idPrefix ?? throw new ArgumentNullException(nameof(idPrefix));
            ResolvedName = resolvedName ?? throw new ArgumentNullException(nameof(resolvedName));
            ViewContext = viewContext;
            AspFor = aspFor;
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
        public (bool isPageHeading, IDictionary<string, string> attributes, IHtmlContent content)? Legend { get; private set; }

        public bool TrySetLegend(bool isPageHeading, IDictionary<string, string> attributes, IHtmlContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (Legend != null)
            {
                return false;
            }

            Legend = (isPageHeading, attributes, content);
            return true;
        }
    }

    internal class RadiosItemContext
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

    internal class RadiosFieldset
    {
        public IDictionary<string, string> Attributes { get; set; }
        public bool? LegendIsPageHeading { get; set; }
        public IHtmlContent LegendContent { get; set; }
        public IDictionary<string, string> LegendAttributes { get; set; }
    }
}
