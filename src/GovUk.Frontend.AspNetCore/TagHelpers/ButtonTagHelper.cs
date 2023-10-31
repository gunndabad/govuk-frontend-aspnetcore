using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS button component that renders a &lt;button&gt; element.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(ComponentGenerator.ButtonElement)]
public class ButtonTagHelper : TagHelper
{
    internal const string TagName = "govuk-button";

    private const string DisabledAttributeName = "disabled";
    private const string IdAttributeName = "id";
    private const string IsStartButtonAttributeName = "is-start-button";
    private const string PreventDoubleClickAttributeName = "prevent-double-click";
    private const string TypeAttributeName = "type";

    private readonly IGovUkHtmlGenerator _htmlGenerator;

    /// <summary>
    /// Creates a new <see cref="ButtonTagHelper"/>.
    /// </summary>
    public ButtonTagHelper(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
        : this(htmlGenerator: null)
    {
    }

    internal ButtonTagHelper(IGovUkHtmlGenerator? htmlGenerator)
    {
        _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
    }

    /// <summary>
    /// Whether the button should be disabled.
    /// </summary>
    /// <remarks>
    /// The default is <c>false</c>.
    /// </remarks>
    [HtmlAttributeName(DisabledAttributeName)]
    public bool Disabled { get; set; } = ComponentGenerator.ButtonDefaultDisabled;

    /// <summary>
    /// The <c>id</c> attribute.
    /// </summary>
    [HtmlAttributeName(IdAttributeName)]
    public string? Id { get; set; }

    /// <summary>
    /// Whether this button is the main call to action on your service's start page.
    /// </summary>
    /// <remarks>
    /// The default is <c>false</c>.
    /// </remarks>
    [HtmlAttributeName(IsStartButtonAttributeName)]
    public bool IsStartButton { get; set; } = ComponentGenerator.ButtonDefaultIsStartButton;

    /// <summary>
    /// Whether to prevent accidental double clicks on submit buttons from submitting forms multiple times.
    /// </summary>
    [HtmlAttributeName(PreventDoubleClickAttributeName)]
    public bool? PreventDoubleClick { get; set; }

    /// <summary>
    /// The <c>type</c> attribute for the generated <c>button</c> element.
    /// </summary>
    [HtmlAttributeName(TypeAttributeName)]
    public string? Type { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var attributes = output.Attributes.ToAttributeDictionary();

        if (Type is not null)
        {
            attributes.Add("type", Type);
        }

        var tagBuilder = _htmlGenerator.GenerateButton(
            IsStartButton,
            Disabled,
            PreventDoubleClick,
            Id,
            childContent,
            attributes);

        output.TagName = tagBuilder.TagName;
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Clear();
        output.MergeAttributes(tagBuilder);
        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
    }
}
