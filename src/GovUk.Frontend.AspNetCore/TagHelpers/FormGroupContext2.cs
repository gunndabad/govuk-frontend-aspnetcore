using System;
using System.Collections.Immutable;
using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal abstract class FormGroupContext2
{
    internal record LabelInfo(bool IsPageHeading, ImmutableDictionary<string, string?> Attributes, string? Html);

    internal record HintInfo(ImmutableDictionary<string, string?> Attributes, string? Html);

    internal record ErrorMessageInfo(string? VisuallyHiddenText, ImmutableDictionary<string, string?> Attributes, string? Html);

    // internal for testing
    internal LabelInfo? Label;
    internal HintInfo? Hint;
    internal ErrorMessageInfo? ErrorMessage;

    protected abstract string ErrorMessageTagName { get; }

    protected abstract string HintTagName { get; }

    protected abstract string LabelTagName { get; }

    protected abstract string RootTagName { get; }

    public LabelOptions GetLabelOptions(
        ModelExpression? aspFor,
        ViewContext viewContext,
        IModelHelper modelHelper,
        string inputId,
        string aspForAttributeName)
    {
        string? html = Label?.Html;

        if (html is null)
        {
            if (aspFor is null)
            {
                throw new InvalidOperationException(
                    $"Label content must be specified when the '{aspForAttributeName}' attribute is not specified.");
            }

            var displayName = modelHelper.GetDisplayName(aspFor!.ModelExplorer, aspFor.Name) ??
                throw new InvalidOperationException($"Cannot deduce content for the <{LabelTagName}>.");

            html = HtmlEncoder.Default.Encode(displayName);
        }

        var attributes = (Label?.Attributes ?? ImmutableDictionary<string, string?>.Empty)
            .Remove("class", out var classes);

        return new LabelOptions()
        {
            Text = null,
            Html = html,
            For = inputId,
            IsPageHeading = Label?.IsPageHeading,
            Classes = classes,
            Attributes = attributes
        };
    }

    public HintOptions? GetHintOptions(ModelExpression? aspFor, IModelHelper modelHelper)
    {
        string? html = Hint?.Html;

        if (html is null && aspFor is not null)
        {
            var description = modelHelper.GetDescription(aspFor.ModelExplorer);

            if (description is not null)
            {
                html = HtmlEncoder.Default.Encode(description);
            }
        }

        if (html is null)
        {
            if (Hint is not null)
            {
                throw new InvalidOperationException($"Cannot deduce content for the <{HintTagName}>.");
            }

            return null;
        }

        var attributes = (Hint?.Attributes ?? ImmutableDictionary<string, string?>.Empty)
            .Remove("class", out var classes);

        return new HintOptions()
        {
            Text = null,
            Html = html,
            Id = null,
            Classes = classes,
            Attributes = attributes
        };
    }

    public ErrorMessageOptions? GetErrorMessageOptions(ModelExpression? aspFor, ViewContext viewContext, IModelHelper modelHelper)
    {
        string? html = ErrorMessage?.Html;

        if (html is null && aspFor is not null)
        {
            var validationMessage = modelHelper.GetValidationMessage(viewContext, aspFor.ModelExplorer, aspFor.Name);

            if (validationMessage is not null)
            {
                html = HtmlEncoder.Default.Encode(validationMessage);
            }
        }

        if (html is null)
        {
            return null;
        }

        var attributes = (ErrorMessage?.Attributes ?? ImmutableDictionary<string, string?>.Empty)
            .Remove("class", out var classes);

        return new ErrorMessageOptions()
        {
            Text = null,
            Html = html,
            Id = null,
            VisuallyHiddenText = ErrorMessage?.VisuallyHiddenText,
            Classes = classes,
            Attributes = attributes
        };
    }

    public virtual void SetErrorMessage(
        string? visuallyHiddenText,
        ImmutableDictionary<string, string?> attributes,
        string? html)
    {
        if (ErrorMessage is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(ErrorMessageTagName, RootTagName);
        }

        ErrorMessage = new ErrorMessageInfo(visuallyHiddenText, attributes, html);
    }

    public virtual void SetHint(ImmutableDictionary<string, string?> attributes, string? html)
    {
        if (Hint is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(HintTagName, RootTagName);
        }

        if (ErrorMessage is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(HintTagName, ErrorMessageTagName);
        }

        Hint = new HintInfo(attributes, html);
    }

    public virtual void SetLabel(bool isPageHeading, ImmutableDictionary<string, string?> attributes, string? html)
    {
        if (Label is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(LabelTagName!, RootTagName);
        }

        if (Hint is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(LabelTagName, HintTagName);
        }

        if (ErrorMessage is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(LabelTagName, ErrorMessageTagName);
        }

        Label = new LabelInfo(isPageHeading, attributes, html);
    }
}
