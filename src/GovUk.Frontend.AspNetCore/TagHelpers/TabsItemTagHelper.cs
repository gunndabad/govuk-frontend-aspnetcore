using System.Diagnostics.CodeAnalysis;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents an item in a GDS tabs component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = TabsTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.TabsItemPanel)]
public class TabsItemTagHelper : TagHelper
{
    internal const string TagName = "govuk-tabs-item";
    internal const string IdAttributeName = "id";

    private const string LabelAttributeName = "label";
    private const string LinkAttributesPrefix = "link-";

    /// <summary>
    /// The <c>id</c> attribute for the tab.
    /// </summary>
    /// <remarks>
    /// Required unless <see cref="TabsTagHelper.IdPrefix"/> is specified on the parent <see cref="TabsTagHelper"/>.
    /// </remarks>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

    /// <summary>
    /// The text label of the tab.
    /// </summary>
    [HtmlAttributeName(LabelAttributeName)]
    [DisallowNull]
    public string? Label { get; set; }

    /// <summary>
    /// Additional attributes to add to the generated link to this tab.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = LinkAttributesPrefix)]
    public IDictionary<string, string?>? LinkAttributes { get; set; } = new Dictionary<string, string?>();

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Label is null)
        {
            throw ExceptionHelper.TheAttributeMustBeSpecified(LabelAttributeName);
        }

        var tabsContext = context.GetContextItem<TabsContext>();

        if (Id is null && !tabsContext.HaveIdPrefix)
        {
            throw new InvalidOperationException(
                $"Item must have the '{IdAttributeName}' attribute specified when parent's 'id-prefix' is not specified.");
        }

        var content = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            content = output.Content;
        }

        var attributes = new AttributeCollection(output.Attributes);
        var itemAttributes = new AttributeCollection(LinkAttributes);

        tabsContext.AddItem(new TabsOptionsItem()
        {
            Id = Id,
            Label = Label,
            Attributes = itemAttributes,
            Panel = new TabsOptionsItemPanel()
            {
                Text = null,
                Html = content.ToTemplateString(),
                Attributes = attributes
            }
        });

        output.SuppressOutput();
    }
}
