using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS tabs component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(TabsItemTagHelper.TagName)]
[OutputElementHint(ComponentGenerator.TabsElement)]
public class TabsTagHelper : TagHelper
{
    internal const string TagName = "govuk-tabs";

    private const string IdAttributeName = "id";
    private const string IdPrefixAttributeName = "id-prefix";
    private const string TitleAttributeName = "title";

    private readonly IGovUkHtmlGenerator _htmlGenerator;

    /// <summary>
    /// Creates a new <see cref="TabsTagHelper"/>.
    /// </summary>
    public TabsTagHelper()
        : this(htmlGenerator: null)
    {
    }

    internal TabsTagHelper(IGovUkHtmlGenerator? htmlGenerator)
    {
        _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
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
        var tabsContext = new TabsContext(haveIdPrefix: IdPrefix != null);

        using (context.SetScopedContextItem(tabsContext))
        {
            await output.GetChildContentAsync();
        }

        var tagBuilder = _htmlGenerator.GenerateTabs(
            Id,
            IdPrefix,
            Title ?? ComponentGenerator.TabsDefaultTitle,
            output.Attributes.ToAttributeDictionary(),
            tabsContext.Items);

        output.TagName = tagBuilder.TagName;
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Clear();
        output.MergeAttributes(tagBuilder);
        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
    }
}
