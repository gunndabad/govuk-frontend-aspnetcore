using System;
using System.Collections.Generic;
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
[RestrictChildren(CheckboxesFieldsetTagHelper.TagName, CheckboxesItemTagHelper.TagName, CheckboxesItemDividerTagHelper.TagName, HintTagName, ErrorMessageTagName)]
[OutputElementHint(ComponentGenerator.FormGroupElement)]
public class CheckboxesTagHelper : FormGroupTagHelperBase
{
    internal const string ErrorMessageTagName = "govuk-checkboxes-error-message";
    internal const string HintTagName = "govuk-checkboxes-hint";
    internal const string TagName = "govuk-checkboxes";

    private const string AttributesPrefix = "checkboxes-";
    private const string IdPrefixAttributeName = "id-prefix";
    private const string NameAttributeName = "name";

    /// <summary>
    /// Creates a new <see cref="CheckboxesTagHelper"/>.
    /// </summary>
    public CheckboxesTagHelper()
        : this(htmlGenerator: null, modelHelper: null)
    {
    }

    internal CheckboxesTagHelper(IGovUkHtmlGenerator? htmlGenerator = null, IModelHelper? modelHelper = null)
        : base(
              htmlGenerator ?? new ComponentGenerator(),
              modelHelper ?? new DefaultModelHelper())
    {
    }

    /// <summary>
    /// Additional attributes for the container element that wraps the items.
    /// </summary>
    [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
    public IDictionary<string, string?>? CheckboxesAttributes { get; set; } = new Dictionary<string, string?>();

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

    private protected override FormGroupContext CreateFormGroupContext() => new CheckboxesContext(Name, For);

    private protected override IHtmlContent GenerateFormGroupContent(
        TagHelperContext tagHelperContext,
        FormGroupContext formGroupContext,
        TagHelperOutput tagHelperOutput,
        IHtmlContent childContent,
        out bool haveError)
    {
        var checkboxesContext = tagHelperContext.GetContextItem<CheckboxesContext>();

        var contentBuilder = new HtmlContentBuilder();

        var hint = GenerateHint(tagHelperContext, formGroupContext);
        if (hint != null)
        {
            contentBuilder.AppendHtml(hint);
        }

        var errorMessage = GenerateErrorMessage(tagHelperContext, formGroupContext);
        if (errorMessage != null)
        {
            contentBuilder.AppendHtml(errorMessage);
        }

        haveError = errorMessage != null;
        var haveFieldset = checkboxesContext.Fieldset != null;

        var checkboxesTagBuilder = GenerateCheckboxes();
        contentBuilder.AppendHtml(checkboxesTagBuilder);

        if (haveFieldset)
        {
            var resolvedFieldsetLegendContent = ResolveFieldsetLegendContent(checkboxesContext.Fieldset!);

            return Generator.GenerateFieldset(
                DescribedBy,
                role: null,
                checkboxesContext.Fieldset!.Legend?.IsPageHeading,
                resolvedFieldsetLegendContent,
                checkboxesContext.Fieldset.Legend?.Attributes,
                content: contentBuilder,
                checkboxesContext.Fieldset.Attributes);
        }

        return contentBuilder;

        TagBuilder GenerateCheckboxes()
        {
            var resolvedIdPrefix = ResolveIdPrefix();
            TryResolveName(out var resolvedName);

            return Generator.GenerateCheckboxes(
                resolvedIdPrefix,
                resolvedName,
                DescribedBy,
                haveFieldset,
                items: checkboxesContext.Items,
                attributes: CheckboxesAttributes.ToAttributeDictionary());
        }
    }

    private protected override string ResolveIdPrefix()
    {
        if (IdPrefix != null)
        {
            return IdPrefix;
        }

        if (Name == null && For == null)
        {
            throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                IdPrefixAttributeName,
                NameAttributeName,
                ForAttributeName);
        }

        TryResolveName(out var resolvedName);
        Debug.Assert(resolvedName != null);

        return TagBuilder.CreateSanitizedId(resolvedName!, Constants.IdAttributeDotReplacement);
    }

    private bool TryResolveName([NotNullWhen(true)] out string? name)
    {
        if (Name == null && For == null)
        {
            name = default;
            return false;
        }

        name = Name ?? ModelHelper.GetFullHtmlFieldName(ViewContext!, For!.Name);
        return true;
    }
}
