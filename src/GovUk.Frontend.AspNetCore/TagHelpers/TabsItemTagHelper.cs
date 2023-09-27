using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Represents an item in a GDS tabs component.
/// </summary>
[HtmlTargetElement(TagName, ParentTag = TabsTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.TabsItemPanelElement)]
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
        if (Label == null)
        {
            throw ExceptionHelper.TheAttributeMustBeSpecified(LabelAttributeName);
        }

        var tabsContext = context.GetContextItem<TabsContext>();

        var childContent = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        tabsContext.AddItem(new TabsItem()
        {
            Id = Id,
            Label = Label,
            LinkAttributes = LinkAttributes?.ToAttributeDictionary(),
            PanelAttributes = output.Attributes.ToAttributeDictionary(),
            PanelContent = childContent.Snapshot()
        });

        output.SuppressOutput();
    }
}
