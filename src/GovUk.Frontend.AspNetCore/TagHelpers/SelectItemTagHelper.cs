using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents an item in a GDS select component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = SelectTagHelper.TagName)]
public class SelectItemTagHelper : TagHelper
{
    internal const string TagName = "govuk-select-item";

    private const string DisabledAttributeName = "disabled";
    private const string SelectedAttributeName = "selected";
    private const string ValueAttributeName = "value";

    private readonly IModelHelper _modelHelper;
    private string _value = ComponentGenerator.SelectItemDefaultValue;
    private bool _selected = ComponentGenerator.SelectItemDefaultSelected;
    private bool _selectedSpecified = false;

    /// <summary>
    /// Creates a new <see cref="SelectItemTagHelper"/>.
    /// </summary>
    public SelectItemTagHelper()
        : this(modelHelper: null)
    {
    }

    internal SelectItemTagHelper(IModelHelper? modelHelper = null)
    {
        _modelHelper = modelHelper ?? new DefaultModelHelper();
    }

    /// <summary>
    /// Whether the <c>disabled</c> attribute should be added to the generated <c>option</c> element.
    /// </summary>
    [HtmlAttributeName(DisabledAttributeName)]
    public bool Disabled { get; set; } = ComponentGenerator.SelectItemDefaultDisabled;

    /// <summary>
    /// Whether the item should be selected.
    /// </summary>
    /// <remarks>
    /// If not specified and <see cref="FormGroupTagHelperBase.AspFor"/> is not <c>null</c> on the parent
    /// <see cref="SelectTagHelper"/> then this value will be computed by comparing the <see cref="Value"/>
    /// attribute with the model expression's value.
    /// </remarks>
    [HtmlAttributeName(SelectedAttributeName)]
    public bool Selected
    {
        get => _selected;
        set
        {
            _selectedSpecified = true;
            _selected = value;
        }
    }

    /// <summary>
    /// The <c>value</c> attribute for the item.
    /// </summary>
    [HtmlAttributeName(ValueAttributeName)]
    [DisallowNull]
    public string Value
    {
        get => _value;
        set => _value = Guard.ArgumentNotNull(nameof(value), value);
    }

    /// <summary>
    /// Gets the <see cref="ViewContext"/> of the executing view.
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    [DisallowNull]
    public ViewContext? ViewContext { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var selectContext = context.GetContextItem<SelectContext>();

        var childContent = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var resolvedSelected = !_selectedSpecified && selectContext.HaveModelExpression ?
            _modelHelper.GetModelValue(
                ViewContext!,
                selectContext.AspFor!.ModelExplorer,
                selectContext.AspFor.Name) == Value :
            _selected;

        selectContext.AddItem(new SelectItem()
        {
            Attributes = output.Attributes.ToAttributeDictionary(),
            Content = childContent.Snapshot(),
            Disabled = Disabled,
            Selected = resolvedSelected,
            Value = Value
        });

        output.SuppressOutput();
    }
}
