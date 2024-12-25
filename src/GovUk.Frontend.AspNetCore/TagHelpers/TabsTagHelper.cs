using System;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS tabs component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(TabsItemTagHelper.TagName, TabsItemTagHelper.ShortTagName)]
[OutputElementHint(DefaultComponentGenerator.TabsElement)]
public class TabsTagHelper : TagHelper
{
    internal const string TagName = "govuk-tabs";

    private const string IdAttributeName = "id";
    private const string IdPrefixAttributeName = "id-prefix";
    private const string TitleAttributeName = "title";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="BackLinkTagHelper"/>.
    /// </summary>
    public TabsTagHelper(IComponentGenerator componentGenerator)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        _componentGenerator = componentGenerator;
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
    /// If not specified, <c>Contents</c> will be used.
    /// </remarks>
    [HtmlAttributeName(TitleAttributeName)]
    public string? Title { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var tabsContext = new TabsContext(haveIdPrefix: IdPrefix != null);

        using (context.SetScopedContextItem(tabsContext))
        {
            await output.GetChildContentAsync();
        }

        var attributes = new EncodedAttributesDictionary(output.Attributes);
        attributes.Remove("class", out var classes);

        var component = _componentGenerator.GenerateTabs(new TabsOptions
        {
            Id = Id.ToHtmlContent(),
            IdPrefix = IdPrefix.ToHtmlContent(),
            Title = Title.ToHtmlContent(),
            Items = tabsContext.Items,
            Classes = classes,
            Attributes = attributes
        });

        component.WriteTo(output);
    }
}
