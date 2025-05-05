using System;
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal abstract class FormGroupContext2
{
    internal record LabelInfo(bool IsPageHeading, EncodedAttributesDictionary Attributes, IHtmlContent? Html, string TagName);

    internal record HintInfo(EncodedAttributesDictionary Attributes, IHtmlContent? Html, string TagName);

    internal record ErrorMessageInfo(IHtmlContent? VisuallyHiddenText, EncodedAttributesDictionary Attributes, IHtmlContent? Html, string TagName);

    // internal for testing
    internal LabelInfo? Label;
    internal HintInfo? Hint;
    internal ErrorMessageInfo? ErrorMessage;

    protected abstract IReadOnlyCollection<string> ErrorMessageTagNames { get; }

    protected abstract IReadOnlyCollection<string> HintTagNames { get; }

    protected abstract IReadOnlyCollection<string> LabelTagNames { get; }

    protected abstract string RootTagName { get; }

    public virtual LabelOptions GetLabelOptions(
        ModelExpression? @for,
        ViewContext viewContext,
        IModelHelper modelHelper,
        IHtmlContent inputId,
        string forAttributeName)
    {
        IHtmlContent? html = Label?.Html;

        if (html is null)
        {
            if (@for is null)
            {
                throw new InvalidOperationException(
                    $"Label content must be specified when the '{forAttributeName}' attribute is not specified.");
            }

            var displayName = modelHelper.GetDisplayName(@for!.ModelExplorer, @for.Name) ??
                throw new InvalidOperationException("Cannot deduce content for the label.");

            html = new HtmlString(displayName);
        }

        var attributes = Label?.Attributes.Clone() ?? new EncodedAttributesDictionary();
        attributes.Remove("class", out var classes);

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

    public HintOptions? GetHintOptions(ModelExpression? @for, IModelHelper modelHelper)
    {
        IHtmlContent? html = Hint?.Html;

        if (html is null && @for is not null)
        {
            var description = modelHelper.GetDescription(@for.ModelExplorer);

            if (description is not null)
            {
                html = new HtmlString(description);
            }
        }

        if (html is null)
        {
            if (Hint is not null)
            {
                throw new InvalidOperationException("Cannot deduce content for the hint.");
            }

            return null;
        }

        var attributes = Hint?.Attributes.Clone() ?? new EncodedAttributesDictionary();
        attributes.Remove("class", out var classes);

        return new HintOptions()
        {
            Text = null,
            Html = html,
            Id = null,
            Classes = classes,
            Attributes = attributes
        };
    }

    public ErrorMessageOptions? GetErrorMessageOptions(ModelExpression? @for, ViewContext viewContext, IModelHelper modelHelper, bool? ignoreModelStateErrors)
    {
        IHtmlContent? html = ErrorMessage?.Html;

        if (html is null && @for is not null && ignoreModelStateErrors != true)
        {
            var validationMessage = modelHelper.GetValidationMessage(viewContext, @for.ModelExplorer, @for.Name);

            if (validationMessage is not null)
            {
                html = new HtmlString(validationMessage);
            }
        }

        if (html is null)
        {
            return null;
        }

        var attributes = ErrorMessage?.Attributes.Clone() ?? new EncodedAttributesDictionary();
        attributes.Remove("class", out var classes);

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
        IHtmlContent? visuallyHiddenText,
        EncodedAttributesDictionary attributes,
        IHtmlContent? html,
        string tagName)
    {
        if (ErrorMessage is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(ErrorMessageTagNames, RootTagName);
        }

        ErrorMessage = new ErrorMessageInfo(visuallyHiddenText, attributes, html, tagName);
    }

    public virtual void SetHint(
        EncodedAttributesDictionary attributes,
        IHtmlContent? html,
        string tagName)
    {
        if (Hint is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(HintTagNames, RootTagName);
        }

        if (ErrorMessage is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(tagName, ErrorMessage.TagName);
        }

        Hint = new HintInfo(attributes, html, tagName);
    }

    public virtual void SetLabel(
        bool isPageHeading,
        EncodedAttributesDictionary attributes,
        IHtmlContent? html,
        string tagName)
    {
        if (Label is not null)
        {
            throw ExceptionHelper.OnlyOneElementIsPermittedIn(LabelTagNames!, RootTagName);
        }

        if (Hint is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(tagName, Hint.TagName);
        }

        if (ErrorMessage is not null)
        {
            throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(tagName, ErrorMessage.TagName);
        }

        Label = new LabelInfo(isPageHeading, attributes, html, tagName);
    }
}
