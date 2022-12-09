using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents an item in a GDS radios component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = RadiosTagHelper.TagName)]
    [HtmlTargetElement(TagName, ParentTag = RadiosFieldsetTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.RadiosItemElement)]
    public class RadiosItemTagHelper : TagHelper
    {
        internal const string TagName = "govuk-radios-item";

        private const string CheckedAttributeName = "checked";
        private const string DisabledAttributeName = "disabled";
        private const string IdAttributeName = "id";
        private const string InputAttributesPrefix = "input-";
        private const string LabelAttributesPrefix = "label-";
        private const string ValueAttributeName = "value";

        private string? _value;

        /// <summary>
        /// Creates a new <see cref="CheckboxesItemTagHelper"/>.
        /// </summary>
        public RadiosItemTagHelper()
        {
        }

        /// <summary>
        /// Whether the item should be checked.
        /// </summary>
        /// <remarks>
        /// If <c>null</c> and <see cref="FormGroupTagHelperBase.AspFor"/> is not <c>null</c> on the parent <see cref="RadiosTagHelper"/> then the value
        /// will be computed by comparing the specified model expression with <see cref="Value"/>.
        /// The default is <c>false</c>.
        /// </remarks>
        [HtmlAttributeName(CheckedAttributeName)]
        public bool? Checked { get; set; }

        /// <summary>
        /// Whether the <c>disabled</c> attribute should be added to the generated <c>input</c> element.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; } = ComponentGenerator.RadiosItemDefaultDisabled;

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
        [HtmlAttributeName(DictionaryAttributePrefix = InputAttributesPrefix)]
        public IDictionary<string, string?> InputAttributes { get; set; } = new Dictionary<string, string?>();

        /// <summary>
        /// Additional attributes to add to the generated <c>label</c> element.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = LabelAttributesPrefix)]
        public IDictionary<string, string?> LabelAttributes { get; set; } = new Dictionary<string, string?>();

        /// <summary>
        /// The <c>value</c> attribute for the item.
        /// </summary>
        [HtmlAttributeName(ValueAttributeName)]
        [DisallowNull]
        public string? Value
        {
            get => _value;
            set => _value = Guard.ArgumentNotNull(nameof(value), value);
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Value == null)
            {
                throw ExceptionHelper.TheAttributeMustBeSpecified(ValueAttributeName);
            }

            var radiosContext = context.GetContextItem<RadiosContext>();

            var itemContext = new RadiosItemContext();

            TagHelperContent childContent;
            using (context.SetScopedContextItem(itemContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            var resolvedChecked = Checked ??
                (radiosContext.AspFor != null ? (bool?)DoesModelMatchItemValue() : null) ??
                ComponentGenerator.RadiosItemDefaultChecked;

            radiosContext.AddItem(new RadiosItem()
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                Checked = resolvedChecked,
                Conditional = itemContext.Conditional != null ?
                    new RadiosItemConditional()
                    {
                        Content = itemContext.Conditional.Value.Content,
                        Attributes = itemContext.Conditional.Value.Attributes
                    } :
                    null,
                Disabled = Disabled,
                Hint = itemContext.Hint != null ?
                    new RadiosItemHint()
                    {
                        Content = itemContext.Hint.Value.Content,
                        Attributes = itemContext.Hint.Value.Attributes
                    } :
                    null,
                Id = Id,
                InputAttributes = InputAttributes.ToAttributeDictionary(),
                LabelAttributes = LabelAttributes.ToAttributeDictionary(),
                LabelContent = childContent.Snapshot(),
                Value = Value
            });

            output.SuppressOutput();

            bool DoesModelMatchItemValue()
            {
                var modelExpression = radiosContext!.AspFor!;
                object model = modelExpression.Model;

                return model?.ToString() == Value;
            }
        }
    }
}
