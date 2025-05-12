using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS checkboxes component.
/// </summary>
[HtmlTargetElement(TagName)]
[RestrictChildren(RadiosFieldsetTagHelper.TagName, RadiosItemTagHelper.TagName, RadiosItemDividerTagHelper.TagName, HintTagName, ErrorMessageTagName)]
[OutputElementHint(ComponentGenerator.RadiosElement)]
public class RadiosTagHelper : FormGroupTagHelperBase
{
    internal const string ErrorMessageTagName = "govuk-radios-error-message";
    internal const string HintTagName = "govuk-radios-hint";
    internal const string TagName = "govuk-radios";

    private const string IdPrefixAttributeName = "id-prefix";
    private const string NameAttributeName = "name";
    private const string RadiosAttributesPrefix = "radios-";

    /// <summary>
    /// Creates a new <see cref="RadiosTagHelper"/>.
    /// </summary>
    public RadiosTagHelper()
        : this(htmlGenerator: null, modelHelper: null)
    {
    }

    internal RadiosTagHelper(IGovUkHtmlGenerator? htmlGenerator = null, IModelHelper? modelHelper = null)
        : base(
              htmlGenerator ?? new ComponentGenerator(),
              modelHelper ?? new DefaultModelHelper())
    {
    }

    /// <summary>
    /// The prefix to use when generating IDs for the hint, error message and items.
    /// </summary>
    /// <remarks>
    /// Required unless <see cref="FormGroupTagHelperBase.AspFor"/> or <see cref="Name"/> is specified.
    /// </remarks>
    [HtmlAttributeName(IdPrefixAttributeName)]
    public string? IdPrefix { get; set; }

    /// <summary>
    /// The <c>name</c> attribute for the generated <c>input</c> elements.
    /// </summary>
    /// <remarks>
    /// Required unless <see cref="FormGroupTagHelperBase.AspFor"/> or <see cref="IdPrefix"/> is specified.
    /// </remarks>
    [HtmlAttributeName(NameAttributeName)]
    public string? Name { get; set; }

    /// <summary>
    /// Additional attributes for the container element that wraps the items.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = RadiosAttributesPrefix)]
    public IDictionary<string, string?>? RadiosAttributes { get; set; } = new Dictionary<string, string?>();

    private protected override FormGroupContext CreateFormGroupContext() => new RadiosContext(Name, For);

    private protected override IHtmlContent GenerateFormGroupContent(
        TagHelperContext tagHelperContext,
        FormGroupContext formGroupContext,
        TagHelperOutput tagHelperOutput,
        IHtmlContent childContent,
        out bool haveError)
    {
        var radiosContext = tagHelperContext.GetContextItem<RadiosContext>();

        var contentBuilder = new HtmlContentBuilder();

        var hint = GenerateHint(tagHelperContext, formGroupContext);
        if (hint is not null)
        {
            contentBuilder.AppendHtml(hint);
        }

        var errorMessage = GenerateErrorMessage(tagHelperContext, formGroupContext);
        if (errorMessage is not null)
        {
            contentBuilder.AppendHtml(errorMessage);
        }

        haveError = errorMessage is not null;
        var haveFieldset = radiosContext.Fieldset is not null;

        var radiosTagBuilder = GenerateRadios();
        contentBuilder.AppendHtml(radiosTagBuilder);

        if (haveFieldset)
        {
            var resolvedFieldsetLegendContent = ResolveFieldsetLegendContent(radiosContext.Fieldset!);

            return Generator.GenerateFieldset(
                DescribedBy,
                role: null,
                radiosContext.Fieldset!.Legend?.IsPageHeading,
                resolvedFieldsetLegendContent,
                radiosContext.Fieldset.Legend?.Attributes,
                content: contentBuilder,
                radiosContext.Fieldset.Attributes);
        }

        return contentBuilder;

        TagBuilder GenerateRadios()
        {
            var resolvedIdPrefix = ResolveIdPrefix();
            TryResolveName(out var resolvedName);

            return Generator.GenerateRadios(
                resolvedIdPrefix,
                resolvedName,
                items: radiosContext.Items,
                attributes: RadiosAttributes.ToAttributeDictionary());
        }
    }

    private protected override string ResolveIdPrefix()
    {
        if (IdPrefix is not null)
        {
            return IdPrefix;
        }

        if (Name is null && For is null)
        {
            throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                IdPrefixAttributeName,
                NameAttributeName,
                AspForAttributeName);
        }

        TryResolveName(out var resolvedName);
        Debug.Assert(resolvedName is not null);

        return TagBuilder.CreateSanitizedId(resolvedName!, Constants.IdAttributeDotReplacement);
    }

    private bool TryResolveName([NotNullWhen(true)] out string? name)
    {
        if (Name is null && For is null)
        {
            name = default;
            return false;
        }

        name = Name ?? ModelHelper.GetFullHtmlFieldName(ViewContext!, For!.Name);
        return true;
    }
}
