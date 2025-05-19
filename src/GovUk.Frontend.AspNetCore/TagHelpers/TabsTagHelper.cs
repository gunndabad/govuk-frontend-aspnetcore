using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS tabs component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(TabsItemTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.ComponentElementTypes.Tabs)]
public class TabsTagHelper : TagHelper
{
    internal const string TagName = "govuk-tabs";

    private const string IdAttributeName = "id";
    private const string IdPrefixAttributeName = "id-prefix";
    private const string TitleAttributeName = "title";

    private readonly IComponentGenerator _componentGenerator;
    private readonly HtmlEncoder _encoder;

    /// <summary>
    /// Creates a new <see cref="TabsTagHelper"/>.
    /// </summary>
    public TabsTagHelper(IComponentGenerator componentGenerator, HtmlEncoder encoder)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        ArgumentNullException.ThrowIfNull(encoder);
        _componentGenerator = componentGenerator;
        _encoder = encoder;
    }

    /// <summary>
    /// The <c>id</c> attribute for the main tabs component.
    /// </summary>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

    /// <summary>
    /// The prefix to use when generating IDs for the items.
    /// </summary>
    /// <remarks>
    /// Required unless every item specifies the <see cref="TabsItemTagHelper.Id"/>.
    /// </remarks>
    [HtmlAttributeName(IdPrefixAttributeName)]
    public string? IdPrefix { get; set; }

    /// <summary>
    /// The title for the tabs table of contents.
    /// </summary>
    /// <remarks>
    /// The default is 'Contents'.
    /// </remarks>
    [HtmlAttributeName(TitleAttributeName)]
    public string? Title { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var tabsContext = new TabsContext(haveIdPrefix: IdPrefix is not null);

        using (context.SetScopedContextItem(tabsContext))
        {
            await output.GetChildContentAsync();
        }

        var attributes = new AttributeCollection(output.Attributes);
        attributes.Remove("class", out var classes);

        var component = await _componentGenerator.GenerateTabsAsync(new TabsOptions()
        {
            Id = Id,
            IdPrefix = IdPrefix,
            Title = Title,
            Items = tabsContext.Items,
            Classes = classes,
            Attributes = attributes
        });

        output.ApplyComponentHtml(component, _encoder);
    }
}
