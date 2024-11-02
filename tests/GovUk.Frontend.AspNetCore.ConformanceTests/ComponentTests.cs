using System;
using System.Collections.Generic;
using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    private delegate IHtmlContent GenerateFormGroupElement(bool haveError, string describedBy);

    private static readonly IHtmlContent _emptyContent = new HtmlString("");

    private readonly ComponentGenerator _componentGenerator;

    public ComponentTests()
    {
        _componentGenerator = new ComponentGenerator();
    }

    private static void AppendToDescribedBy(ref string describedBy, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (string.IsNullOrEmpty(describedBy))
        {
            describedBy = value;
        }
        else
        {
            describedBy += " " + value;
        }
    }

    private void CheckComponentHtmlMatchesExpectedHtml<TOptions>(
        ComponentTestCaseData<TOptions> testCaseData,
        Func<ComponentGenerator, TOptions, string> generateComponent,
        Predicate<IDiff> excludeDiff = null
    )
    {
        var html = generateComponent(_componentGenerator, testCaseData.Options);

        AssertEx.HtmlEqual(testCaseData.ExpectedHtml, html, excludeDiff);
    }

    private string GenerateFormGroup(
        Label label,
        Hint hint,
        ErrorMessage errorMessage,
        FormGroup formGroup,
        Fieldset fieldset,
        GenerateFormGroupElement generateElement
    )
    {
        var haveError = errorMessage != null;

        string describedBy = null;

        var attributes = new AttributeDictionary().MergeAttribute("class", formGroup?.Classes);

        var contentBuilder = new HtmlContentBuilder();

        if (label != null)
        {
            var labelTagBuilder = BuildLabel(_componentGenerator, label);
            contentBuilder.AppendHtml(labelTagBuilder);
        }

        if (hint != null)
        {
            var hintTagBuilder = BuildHint(_componentGenerator, hint);
            contentBuilder.AppendHtml(hintTagBuilder);

            AppendToDescribedBy(ref describedBy, hint.Id);
        }

        if (errorMessage != null)
        {
            var errorMessageTagBuilder = BuildErrorMessage(_componentGenerator, errorMessage);
            contentBuilder.AppendHtml(errorMessageTagBuilder);

            AppendToDescribedBy(ref describedBy, errorMessage.Id);
        }

        var element = generateElement(haveError, describedBy);
        contentBuilder.AppendHtml(element);

        IHtmlContent content = contentBuilder;

        if (fieldset != null)
        {
            AppendToDescribedBy(ref describedBy, fieldset.DescribedBy);

            content = _componentGenerator.GenerateFieldset(
                describedBy,
                role: fieldset.Role,
                fieldset.Legend?.IsPageHeading ?? ComponentGenerator.FieldsetLegendDefaultIsPageHeading,
                legendContent: TextOrHtmlHelper.GetHtmlContent(fieldset.Legend?.Text, fieldset.Legend?.Html),
                legendAttributes: new AttributeDictionary().MergeAttribute("class", fieldset.Legend?.Classes),
                content: contentBuilder,
                attributes: fieldset.Attributes.ToAttributesDictionary().MergeAttribute("class", fieldset.Classes)
            );
        }

        return _componentGenerator.GenerateFormGroup(haveError, content, attributes).ToHtmlString();
    }
}
