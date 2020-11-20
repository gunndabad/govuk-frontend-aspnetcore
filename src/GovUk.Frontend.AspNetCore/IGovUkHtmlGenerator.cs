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
            int headingLevel,
            IDictionary<string, string> attributes,
            IEnumerable<AccordionItem> items);

        TagBuilder GenerateAnchor(string href);

        TagBuilder GenerateBackLink(string href, IHtmlContent content, IDictionary<string, string> attributes);

        TagBuilder GenerateBreadcrumbs(
            bool collapseOnMobile,
            IDictionary<string, string> attributes,
            IEnumerable<BreadcrumbsItem> items);

        TagBuilder GenerateButton(
            string name,
            string type,
            string value,
            bool isStartButton,
            bool disabled,
            bool preventDoubleClick,
            string formAction,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        TagBuilder GenerateButtonLink(
            string href,
            bool isStartButton,
            bool disabled,
            IHtmlContent content,
            IDictionary<string, string> attributes);

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

        TagBuilder GenerateDateInput(
            string id,
            bool disabled,
            DateInputItem day,
            DateInputItem month,
            DateInputItem year,
            IDictionary<string, string> attributes);

        TagBuilder GenerateDetails(
            bool open,
            string id,
            IHtmlContent summaryContent,
            IDictionary<string, string> summaryAttributes,
            IHtmlContent text,
            IDictionary<string, string> textAttributes,
            IDictionary<string, string> attributes);

        TagBuilder GenerateErrorMessage(
            string visuallyHiddenText,
            string id,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        TagBuilder GenerateErrorSummary(
            IHtmlContent titleContent,
            IDictionary<string, string> titleAttributes,
            IHtmlContent descriptionContent,
            IDictionary<string, string> descriptionAttributes,
            IDictionary<string, string> attributes,
            IEnumerable<ErrorSummaryItem> items);

        TagBuilder GenerateFieldset(
            string describedBy,
            string role,
            bool? legendIsPageHeading,
            IHtmlContent legendContent,
            IDictionary<string, string> legendAttributes,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        TagBuilder GenerateFileUpload(
            bool haveError,
            string id,
            string name,
            string describedBy,
            IDictionary<string, string> attributes);

        TagBuilder GenerateFormGroup(bool haveError, IHtmlContent content, IDictionary<string, string> attributes);

        TagBuilder GenerateHint(string id, IHtmlContent content, IDictionary<string, string> attributes);

        TagBuilder GenerateInsetText(string id, IHtmlContent content, IDictionary<string, string> attributes);

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
            bool? spellcheck,
            bool disabled,
            IDictionary<string, string> attributes,
            IHtmlContent prefixContent,
            IDictionary<string, string> prefixAttributes,
            IHtmlContent suffixContent,
            IDictionary<string, string> suffixAttributes);

        TagBuilder GenerateLabel(
            string @for,
            bool isPageHeading,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        TagBuilder GeneratePanel(
            int titleHeadingLevel,
            IHtmlContent titleContent,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        TagBuilder GeneratePhaseBanner(
            IHtmlContent tagContent,
            IDictionary<string, string> tabAttributes,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        TagBuilder GenerateRadios(
            string name,
            bool isConditional,
            IEnumerable<RadiosItemBase> items,
            IDictionary<string, string> attributes);

        TagBuilder GenerateSelect(
            bool haveError,
            string id,
            string name,
            string describedBy,
            bool disabled,
            IEnumerable<SelectListItem> items,
            IDictionary<string, string> attributes);

        TagBuilder GenerateSkipLink(string href, IHtmlContent content, IDictionary<string, string> attributes);

        TagBuilder GenerateSummaryList(IDictionary<string, string> attributes, IEnumerable<SummaryListRow> rows);

        TagBuilder GenerateTabs(
            string id,
            string title,
            IDictionary<string, string> attributes,
            IEnumerable<TabsItem> items);

        TagBuilder GenerateTag(IHtmlContent content, IDictionary<string, string> attributes);

        TagBuilder GenerateTextArea(
            bool haveError,
            string id,
            string name,
            int rows,
            string describedBy,
            string autocomplete,
            bool? spellcheck,
            bool disabled,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        TagBuilder GenerateWarningText(
            string iconFallbackText,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        string GetDisplayName(ViewContext viewContext, ModelExplorer modelExplorer, string expression);

        string GetFullHtmlFieldName(ViewContext viewContext, string expression);

        string GetModelValue(ViewContext viewContext, ModelExplorer modelExplorer, string expression);

        string GetValidationMessage(ViewContext viewContext, ModelExplorer modelExplorer, string expression);
    }
}