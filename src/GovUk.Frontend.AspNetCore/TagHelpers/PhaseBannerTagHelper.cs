using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS phase banner component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(DefaultComponentGenerator.PhaseBannerElement)]
public class PhaseBannerTagHelper : TagHelper
{
    internal const string TagName = "govuk-phase-banner";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a <see cref="PhaseBannerTagHelper"/>.
    /// </summary>
    public PhaseBannerTagHelper(IComponentGenerator componentGenerator)
    {
        _componentGenerator = componentGenerator;
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var phaseBannerContext = new PhaseBannerContext();

        IHtmlContent childContent;

        using (context.SetScopedContextItem(phaseBannerContext))
        {
            childContent = await output.GetChildContentAsync();
        }

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var attributes = output.Attributes.ToEncodedAttributeDictionary()
            .Remove("class", out var classes);

        var tagOptions = phaseBannerContext.GetTagOptions();

        var component = _componentGenerator.GeneratePhaseBanner(new PhaseBannerOptions()
        {
            Text = null,
            Html = childContent.ToHtmlString(),
            Tag = tagOptions,
            Classes = classes,
            Attributes = attributes
        });

        output.WriteComponent(component);
    }
}
