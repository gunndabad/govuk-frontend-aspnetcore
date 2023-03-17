using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
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
            AttributeDictionary attributes,
            IEnumerable<AccordionItem> items);

        TagBuilder GenerateAnchor(string href);

        TagBuilder GenerateBackLink(IHtmlContent content, AttributeDictionary attributes);

        TagBuilder GenerateBreadcrumbs(
            bool collapseOnMobile,
            AttributeDictionary attributes,
            IEnumerable<BreadcrumbsItem> items);

        TagBuilder GenerateButton(
            bool isStartButton,
            bool disabled,
            bool preventDoubleClick,
            IHtmlContent content,
            AttributeDictionary attributes);

        TagBuilder GenerateButtonLink(
            bool isStartButton,
            bool disabled,
            IHtmlContent content,
            AttributeDictionary attributes);

        TagBuilder GenerateCharacterCount(
            string textAreaId,
            int? maxLength,
            int? maxWords,
            decimal? threshold,
            IHtmlContent formGroup,
            AttributeDictionary countMessageAttributes);

        TagBuilder GenerateCheckboxes(
            string idPrefix,
            string name,
            string describedBy,
            bool hasFieldset,
            IEnumerable<CheckboxesItemBase> items,
            AttributeDictionary attributes);

        TagBuilder GenerateDateInput(
            string id,
            bool disabled,
            DateInputItem day,
            DateInputItem month,
            DateInputItem year,
            AttributeDictionary attributes);

        TagBuilder GenerateDetails(
            bool open,
            IHtmlContent summaryContent,
            AttributeDictionary summaryAttributes,
            IHtmlContent text,
            AttributeDictionary textAttributes,
            AttributeDictionary attributes);

        TagBuilder GenerateErrorMessage(
            string visuallyHiddenText,
            IHtmlContent content,
            AttributeDictionary attributes);

        TagBuilder GenerateErrorSummary(
            bool disableAutofocus,
            IHtmlContent titleContent,
            AttributeDictionary titleAttributes,
            IHtmlContent descriptionContent,
            AttributeDictionary descriptionAttributes,
            AttributeDictionary attributes,
            IEnumerable<ErrorSummaryItem> items);

        TagBuilder GenerateFieldset(
            string describedBy,
            string role,
            bool? legendIsPageHeading,
            IHtmlContent legendContent,
            AttributeDictionary legendAttributes,
            IHtmlContent content,
            AttributeDictionary attributes);

        TagBuilder GenerateFileUpload(
            bool haveError,
            string id,
            string name,
            string describedBy,
            AttributeDictionary attributes);

        TagBuilder GenerateFormGroup(bool haveError, IHtmlContent content, AttributeDictionary attributes);

        TagBuilder GenerateHint(string id, IHtmlContent content, AttributeDictionary attributes);

        TagBuilder GenerateInsetText(string id, IHtmlContent content, AttributeDictionary attributes);

        TagBuilder GenerateLabel(
            string @for,
            bool isPageHeading,
            IHtmlContent content,
            AttributeDictionary attributes);

        TagBuilder GenerateNotificationBanner(
            NotificationBannerType type,
            string role,
            bool disableAutoFocus,
            string titleId,
            int? titleHeadingLevel,
            IHtmlContent titleContent,
            IHtmlContent content,
            AttributeDictionary attributes);

        TagBuilder GeneratePanel(
            int headingLevel,
            IHtmlContent titleContent,
            IHtmlContent bodyContent,
            AttributeDictionary attributes);

        TagBuilder GeneratePhaseBanner(
            IHtmlContent tagContent,
            AttributeDictionary tabAttributes,
            IHtmlContent content,
            AttributeDictionary attributes);

        TagBuilder GenerateRadios(
            string idPrefix,
            string name,
            IEnumerable<RadiosItemBase> items,
            AttributeDictionary attributes);

        TagBuilder GenerateSelect(
            bool haveError,
            string id,
            string name,
            string describedBy,
            bool disabled,
            IEnumerable<SelectItem> items,
            AttributeDictionary attributes);

        TagBuilder GenerateSkipLink(string href, IHtmlContent content, AttributeDictionary attributes);

        TagBuilder GenerateSummaryList(AttributeDictionary attributes, IEnumerable<SummaryListRow> rows);

        TagBuilder GenerateSummaryCard(SummaryCard summaryCard);

        TagBuilder GenerateTabs(
            string id,
            string title,
            AttributeDictionary attributes,
            IEnumerable<TabsItem> items);

        TagBuilder GenerateTag(IHtmlContent content, AttributeDictionary attributes);

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
            AttributeDictionary attributes);

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
            AttributeDictionary attributes,
            IHtmlContent prefixContent,
            AttributeDictionary prefixAttributes,
            IHtmlContent suffixContent,
            AttributeDictionary suffixAttributes);

        TagBuilder GenerateWarningText(
            string iconFallbackText,
            IHtmlContent content,
            AttributeDictionary attributes);
    }
}
