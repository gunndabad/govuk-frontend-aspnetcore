using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS button component that renders an &lt;a&gt; element.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(Element)]
public class ButtonLinkTagHelper : TagHelper
{
    internal const string TagName = "govuk-button-link";
    internal const string Element = "a";

    private const string IdAttributeName = "id";
    private const string IsStartButtonAttributeName = "is-start-button";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a <see cref="ButtonLinkTagHelper"/>.
    /// </summary>
    public ButtonLinkTagHelper(IComponentGenerator componentGenerator)
    {
        _componentGenerator = componentGenerator;
    }

    /// <summary>
    /// The <c>id</c> attribute for the generated <c>button</c> element..
    /// </summary>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

    /// <summary>
    /// Whether this button is the main call to action on your service's start page.
    /// </summary>
    [HtmlAttributeName(IsStartButtonAttributeName)]
    public bool IsStartButton { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var content = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            content = output.Content;
        }

        var attributes = output.Attributes.ToEncodedAttributeDictionary()
            .Remove("class", out var classes)
            .Remove("href", out var href);

        var component = _componentGenerator.GenerateButton(new ButtonOptions()
        {
            Element = Element,
            Html = content?.ToHtmlString(),
            Href = href,
            Classes = classes,
            Attributes = attributes,
            IsStartButton = IsStartButton,
            Id = Id
        });

        output.WriteComponent(component);
    }
}
