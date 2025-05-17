using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS button component that renders a &lt;button&gt; element.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(Element)]
public class ButtonTagHelper : TagHelper
{
    internal const string TagName = "govuk-button";
    internal const string Element = "button";

    private const string DisabledAttributeName = "disabled";
    private const string IdAttributeName = "id";
    private const string IsStartButtonAttributeName = "is-start-button";
    private const string NameAttributeName = "name";
    private const string PreventDoubleClickAttributeName = "prevent-double-click";
    private const string TypeAttributeName = "type";
    private const string ValueAttributeName = "value";

    private readonly IComponentGenerator _componentGenerator;

    /// <summary>
    /// Creates a new <see cref="ButtonTagHelper"/>.
    /// </summary>
    public ButtonTagHelper(IComponentGenerator componentGenerator)
    {
        ArgumentNullException.ThrowIfNull(componentGenerator);
        _componentGenerator = componentGenerator;
    }

    /// <summary>
    /// Whether the button should be disabled.
    /// </summary>
    [HtmlAttributeName(DisabledAttributeName)]
    public bool? Disabled { get; set; }

    /// <summary>
    /// The <c>id</c> attribute for the generated <c>button</c> element.
    /// </summary>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

    /// <summary>
    /// The <c>name</c> attribute for the generated <c>button</c> element.
    /// </summary>
    [HtmlAttributeName(NameAttributeName)]
    public string? Name { get; set; }

    /// <summary>
    /// Whether this button is the main call to action on your service's start page.
    /// </summary>
    [HtmlAttributeName(IsStartButtonAttributeName)]
    public bool? IsStartButton { get; set; }

    /// <summary>
    /// Whether to prevent accidental double clicks on submit buttons from submitting forms multiple times.
    /// </summary>
    /// <remarks>
    /// The default is set for the application in <see cref="GovUkFrontendAspNetCoreOptions.DefaultButtonPreventDoubleClick"/>.
    /// </remarks>
    [HtmlAttributeName(PreventDoubleClickAttributeName)]
    public bool? PreventDoubleClick { get; set; }

    /// <summary>
    /// The <c>type</c> attribute for the generated <c>button</c> element.
    /// </summary>
    [HtmlAttributeName(TypeAttributeName)]
    public string? Type { get; set; }

    /// <summary>
    /// The <c>value</c> attribute for the generated <c>button</c> element.
    /// </summary>
    [HtmlAttributeName(ValueAttributeName)]
    public string? Value { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var content = (await output.GetChildContentAsync()).Snapshot();

        if (output.Content.IsModified)
        {
            content = output.Content;
        }

        var attributes = new AttributeCollection(output.Attributes);
        attributes.Remove("class", out var classes);

        if (output.Attributes.ContainsName("formaction"))
        {
            attributes.Set("formaction", output.GetUrlAttribute("formaction")!);
        }

        var component = await _componentGenerator.GenerateButtonAsync(new ButtonOptions()
        {
            Element = Element,
            Html = content.ToTemplateString(),
            Name = Name,
            Type = Type,
            Value = Value,
            Disabled = Disabled,
            Classes = classes,
            Attributes = attributes,
            PreventDoubleClick = PreventDoubleClick,
            IsStartButton = IsStartButton,
            Id = Id
        });

        output.ApplyComponentHtml(component, HtmlEncoder.Default);
    }
}
