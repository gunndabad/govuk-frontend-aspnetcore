using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS fieldset component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(DefaultComponentGenerator.FieldsetElement)]
public class FieldsetTagHelper : TagHelper
{
    internal const string TagName = "govuk-fieldset";

    private const string DescribedByAttributeName = "described-by";
    private const string RoleAttributeName = "role";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="FieldsetTagHelper"/>.
    /// </summary>
    public FieldsetTagHelper(IComponentGenerator componentGenerator)
    {
        _componentGenerator = componentGenerator;
    }

    /// <summary>
    /// One or more element IDs to add to the <c>aria-describedby</c> attribute.
    /// </summary>
    [HtmlAttributeName(DescribedByAttributeName)]
    public string? DescribedBy { get; set; }

    /// <summary>
    /// The <c>role</c> attribute.
    /// </summary>
    [HtmlAttributeName(RoleAttributeName)]
    public string? Role { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var fieldsetContext = new FieldsetContext();

        IHtmlContent childContent;

        using (context.SetScopedContextItem(fieldsetContext))
        {
            childContent = await output.GetChildContentAsync();
        }

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var legendOptions = fieldsetContext.GetLegendOptions();

        var attributes = output.Attributes.ToEncodedAttributeDictionary()
            .Remove("class", out var classes);

        var component = _componentGenerator.GenerateFieldset(new FieldsetOptions()
        {
            DescribedBy = DescribedBy,
            Legend = legendOptions,
            Role = Role,
            Text = null,
            Html = childContent.ToHtmlString(),
            Classes = classes,
            Attributes = attributes
        });

        output.TagName = null;
        output.Attributes.Clear();
        output.Content.AppendHtml(component.ToHtmlString());
    }
}
