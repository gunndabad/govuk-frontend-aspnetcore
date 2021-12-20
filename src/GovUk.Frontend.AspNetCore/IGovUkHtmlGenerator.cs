using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        TagBuilder GenerateBackLink(IHtmlContent content, IDictionary<string, string> attributes);

        TagBuilder GenerateBreadcrumbs(
            bool collapseOnMobile,
            IDictionary<string, string> attributes,
            IEnumerable<BreadcrumbsItem> items);

        TagBuilder GenerateButton(
            bool isStartButton,
            bool disabled,
            bool preventDoubleClick,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        TagBuilder GenerateButtonLink(
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
            string idPrefix,
            string name,
            string describedBy,
            bool hasFieldset,
            IEnumerable<CheckboxesItemBase> items,
            IDictionary<string, string> attributes);

        TagBuilder GenerateDateInput(
            string id,
            bool disabled,
            DateInputItem day,
            DateInputItem month,
            DateInputItem year,
            IDictionary<string, string> attributes);

        TagBuilder GenerateDetails(
            bool open,
            IHtmlContent summaryContent,
            IDictionary<string, string> summaryAttributes,
            IHtmlContent text,
            IDictionary<string, string> textAttributes,
            IDictionary<string, string> attributes);

        TagBuilder GenerateErrorMessage(
            string visuallyHiddenText,
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

        TagBuilder GenerateLabel(
            string @for,
            bool isPageHeading,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        TagBuilder GenerateNotificationBanner(
            NotificationBannerType type,
            string role,
            bool disableAutoFocus,
            string titleId,
            int? titleHeadingLevel,
            IHtmlContent titleContent,
            IHtmlContent content,
            IDictionary<string, string> attributes);

        TagBuilder GeneratePanel(
            int headingLevel,
            IHtmlContent titleContent,
            IHtmlContent bodyContent,
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
            IEnumerable<SelectItem> items,
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

        TagBuilder GenerateTextInput(
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

        TagBuilder GenerateWarningText(
            string iconFallbackText,
            IHtmlContent content,
            IDictionary<string, string> attributes);
    }
}
