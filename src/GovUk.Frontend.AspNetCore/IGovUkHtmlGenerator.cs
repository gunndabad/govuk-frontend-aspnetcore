using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    public interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateAccordion(
            string id,
            IDictionary<string, string> attributes,
            IEnumerable<AccordionItem> items);

        TagBuilder GenerateAnchor(string href);

        TagBuilder GenerateBackLink(string href, IDictionary<string, string> attributes, IHtmlContent content);

        TagBuilder GenerateBreadcrumbs(IDictionary<string, string> attributes, IEnumerable<BreadcrumbsItem> items);

        TagBuilder GenerateButton(
            string name,
            string type,
            string value,
            bool isStartButton,
            bool disabled,
            bool preventDoubleClick,
            IDictionary<string, string> attributes,
            IHtmlContent content);

        TagBuilder GenerateButtonLink(
            string href,
            bool isStartButton,
            bool disabled,
            IDictionary<string, string> attributes,
            IHtmlContent content);

        TagBuilder GenerateCharacterCount(
            string elementId,
            int? maxLength,
            int? maxWords,
            decimal? threshold,
            IHtmlContent formGroup);

        TagBuilder GenerateCheckboxes(
            string name,
            bool isConditional,
            string describedBy,
            IDictionary<string, string> attributes,
            IEnumerable<CheckboxesItem> items);

        TagBuilder GenerateDetails(
            bool open,
            IHtmlContent summary,
            IHtmlContent text,
            IDictionary<string, string> attributes);

        TagBuilder GenerateErrorMessage(
            string visuallyHiddenText,
            string id,
            IDictionary<string, string> attributes,
            IHtmlContent content);

        TagBuilder GenerateErrorSummary(
            IHtmlContent title,
            IHtmlContent description,
            IDictionary<string, string> attributes,
            IEnumerable<ErrorSummaryItem> items);

        TagBuilder GenerateFieldset(
            string describedBy,
            bool isPageHeading,
            string role,
            IDictionary<string, string> attributes,
            IHtmlContent legendContent,
            IDictionary<string, string> legendAttributes,
            IHtmlContent content);

        TagBuilder GenerateFormGroup(bool haveError, IDictionary<string, string> attributes, IHtmlContent content);

        TagBuilder GenerateHint(string id, IDictionary<string, string> attributes, IHtmlContent content);

        TagBuilder GenerateInsetText(string id, IDictionary<string, string> attributes, IHtmlContent content);

        TagBuilder GenerateInput(
            bool haveError,
            string id,
            string name,
            string type,
            string value,
            string describedBy,
            string autocomplete,
            string pattern,
            string inputMode,
            bool disabled,
            IDictionary<string, string> attributes);

        TagBuilder GenerateLabel(
            string @for,
            bool isPageHeading,
            IDictionary<string, string> attributes,
            IHtmlContent content);

        TagBuilder GeneratePanel(
            int? titleHeadingLevel,
            IHtmlContent titleContent,
            IDictionary<string, string> attributes,
            IHtmlContent content);

        TagBuilder GeneratePhaseBanner(
            IDictionary<string, string> tabAttributes,
            IHtmlContent tagContent,
            IDictionary<string, string> attributes,
            IHtmlContent content);

        TagBuilder GenerateRadios(string name, bool isConditional, IEnumerable<RadiosItemBase> items);

        TagBuilder GenerateSkipLink(string href, IDictionary<string, string> attributes, IHtmlContent content);

        TagBuilder GenerateSummaryList(IDictionary<string, string> attributes, IEnumerable<SummaryListRow> rows);

        TagBuilder GenerateTabs(
            string id,
            string title,
            IDictionary<string, string> attributes,
            IEnumerable<TabsItem> items);

        TagBuilder GenerateTag(IDictionary<string, string> attributes, IHtmlContent content);

        TagBuilder GenerateTextArea(
            bool haveError,
            string id,
            string name,
            int? rows,
            string describedBy,
            string autocomplete,
            bool disabled,
            IDictionary<string, string> attributes,
            IHtmlContent content);

        TagBuilder GenerateWarningText(
            IDictionary<string, string> attributes,
            IHtmlContent content,
            string iconFallbackText);

        string GetDisplayName(ViewContext viewContext, ModelExplorer modelExplorer, string expression);

        string GetFullHtmlFieldName(ViewContext viewContext, string expression);

        string GetModelValue(ViewContext viewContext, ModelExplorer modelExplorer, string expression);

        string GetValidationMessage(ViewContext viewContext, ModelExplorer modelExplorer, string expression);
    }
}