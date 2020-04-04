using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-select")]
    [RestrictChildren("govuk-select-fieldset", "govuk-select-item", "govuk-select-hint", "govuk-select-label", "govuk-select-error-message")]
    public class SelectTagHelper : FormGroupTagHelperBase
    {
        private const string AttributesPrefix = "select-";
        private const string DisabledAttributeName = "disabled";
        private const string IdAttributeName = "id";

        public SelectTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Name == null && AspFor == null)
            {
                throw new InvalidOperationException(
                    $"At least one of the '{NameAttributeName}' and '{AspForAttributeName}' attributes must be specified.");
            }

            var selectContext = new SelectContext();
            using (context.SetScopedContextItem(typeof(SelectContext), selectContext))
            {
                await base.ProcessAsync(context, output);
            }
        }

        protected override string GetIdPrefix() => Id;

        protected override TagBuilder GenerateElement(
            TagHelperContext context,
            FormGroupBuilder builder,
            FormGroupElementContext elementContext)
        {
            var selectContext = (SelectContext)context.Items[typeof(SelectContext)];

            return Generator.GenerateSelect(
                elementContext.HaveError,
                ResolvedId,
                ResolvedName,
                DescribedBy,
                Disabled,
                selectContext.Items,
                FormGroupAttributes);
        }
    }

    [HtmlTargetElement("govuk-select-label", ParentTag = "govuk-select")]
    public class SelectLabelTagHelper : FormGroupLabelTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-select-hint", ParentTag = "govuk-select")]
    public class SelectHintTagHelper : FormGroupHintTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-select-error-message", ParentTag = "govuk-select")]
    public class SelectErrorMessageTagHelper : FormGroupErrorMessageTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-select-item", ParentTag = "govuk-select")]
    public class SelectItemTagHelper : TagHelper
    {
        private const string DisabledAttributeName = "disabled";
        private const string SelectedAttributeName = "selected";
        private const string ValueAttributeName = "value";

        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; }

        [HtmlAttributeName(SelectedAttributeName)]
        public bool Selected { get; set; }

        [HtmlAttributeName(ValueAttributeName)]
        public string Value { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var selectContext = (SelectContext)context.Items[typeof(SelectContext)];

            var content = await output.GetChildContentAsync();

            selectContext.AddItem(new SelectListItem()
            {
                Attributes = output.Attributes.ToAttributesDictionary(),
                Content = content.Snapshot(),
                Disabled = Disabled,
                Selected = Selected,
                Value = Value
            });

            output.SuppressOutput();
        }
    }

    internal class SelectContext
    {
        private List<SelectListItem> _items;

        public SelectContext()
        {
            _items = new List<SelectListItem>();
        }

        public IReadOnlyCollection<SelectListItem> Items => _items;

        public void AddItem(SelectListItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }
    }
}
