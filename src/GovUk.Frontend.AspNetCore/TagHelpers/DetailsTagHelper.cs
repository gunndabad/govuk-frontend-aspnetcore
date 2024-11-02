using System;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS details component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(DetailsSummaryTagHelper.TagName, DetailsTextTagHelper.TagName)]
[OutputElementHint(DefaultComponentGenerator.DetailsElement)]
public class DetailsTagHelper : TagHelper
{
    internal const string TagName = "govuk-details";

    private const string IdAttributeName = "id";
    private const string OpenAttributeName = "open";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="DetailsTagHelper"/>.
    /// </summary>
    public DetailsTagHelper(IComponentGenerator componentGenerator)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        _componentGenerator = componentGenerator;
    }

    /// <summary>
    /// The <c>id</c> attribute for the generated <c>details</c> element.
    /// </summary>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

    /// <summary>
    /// Whether the details element should be expanded.
    /// </summary>
    [HtmlAttributeName(OpenAttributeName)]
    public bool? Open { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var detailsContext = new DetailsContext();

        using (context.SetScopedContextItem(detailsContext))
        {
            await output.GetChildContentAsync();
        }

        var (summaryAttributes, summaryHtml) = detailsContext.GetSummaryOptions();
        var (textAttributes, textHtml) = detailsContext.GetTextOptions();

        var attributes = output.Attributes.ToEncodedAttributeDictionary().Remove("class", out var classes);

        var component = _componentGenerator.GenerateDetails(
            new DetailsOptions()
            {
                Id = Id,
                Open = Open,
                SummaryHtml = summaryHtml,
                SummaryText = null,
                Html = textHtml,
                Text = null,
                Classes = classes,
                Attributes = attributes,
                SummaryAttributes = summaryAttributes,
                TextAttributes = textAttributes,
            }
        );

        output.WriteComponent(component);
    }
}
