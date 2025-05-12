using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const bool DateInputDefaultDisabled = false;
    internal const string DateInputDefaultInputMode = "numeric";
    internal const string DateInputDefaultDayLabel = "Day";
    internal const string DateInputDefaultMonthLabel = "Month";
    internal const string DateInputDefaultYearLabel = "Year";

    public TagBuilder GenerateDateInput(
        string id,
        bool disabled,
        DateInputItem day,
        DateInputItem month,
        DateInputItem year,
        AttributeDictionary? attributes)
    {
        Guard.ArgumentNotNull(nameof(id), id);
        Guard.ArgumentNotNull(nameof(day), day);
        Guard.ArgumentNotNull(nameof(month), month);
        Guard.ArgumentNotNull(nameof(year), year);

        var tagBuilder = new TagBuilder("div");
        tagBuilder.MergeOptionalAttributes(attributes);
        tagBuilder.AddCssClass("govuk-date-input");
        tagBuilder.Attributes.Add("id", id);

        tagBuilder.InnerHtml.AppendHtml(CreateDateComponent(day, nameof(day)));
        tagBuilder.InnerHtml.AppendHtml(CreateDateComponent(month, nameof(month)));
        tagBuilder.InnerHtml.AppendHtml(CreateDateComponent(year, nameof(year)));

        return tagBuilder;

        IHtmlContent CreateDateComponent(DateInputItem item, string argName)
        {
            Guard.ArgumentValidNotNull(
                argName,
                $"{argName} is not valid; {nameof(DateInputItem.Id)} cannot be null or empty.",
                item.Id,
                !string.IsNullOrEmpty(item.Id));

            Guard.ArgumentValidNotNull(
                argName,
                $"{argName} is not valid; {nameof(DateInputItem.Name)} cannot be null or empty.",
                item.Name,
                !string.IsNullOrEmpty(item.Name));

            Guard.ArgumentValidNotNull(
                argName,
                $"{argName} is not valid; {nameof(DateInputItem.LabelContent)} cannot be null.",
                item.LabelContent,
                item.LabelContent is not null);

            var div = new TagBuilder("div");
            div.AddCssClass("govuk-date-input__item");

            var itemInput = GenerateTextInput(
                item.HaveError,
                item.Id,
                item.Name,
                type: "text",
                item.Value,
                describedBy: null,
                item.Autocomplete,
                item.Pattern,
                item.InputMode,
                spellcheck: null,
                disabled,
                item.Attributes,
                prefixContent: null,
                prefixAttributes: null,
                suffixContent: null,
                suffixAttributes: null);
            itemInput.MergeCssClass("govuk-date-input__input");

            var itemLabel = GenerateLabel(
                @for: item.Id,
                isPageHeading: false,
                content: item.LabelContent,
                attributes: null)!;
            itemLabel.AddCssClass("govuk-date-input__label");

            var contentBuilder = new HtmlContentBuilder()
                .AppendHtml(itemLabel)
                .AppendHtml(itemInput);

            var itemFormGroup = GenerateFormGroup(
                haveError: false,  // GDS component doesn't add error classes to these form groups
                content: contentBuilder,
                attributes: null);

            div.InnerHtml.AppendHtml(itemFormGroup);

            return div;
        }
    }
}
