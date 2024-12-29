#nullable disable
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore;

internal interface IGovUkHtmlGenerator
{
    TagBuilder GenerateAccordion(
        string id,
        int headingLevel,
        AttributeDictionary attributes,
        bool rememberExpanded,
        string hideAllSectionsText,
        string hideSectionText,
        string hideSectionAriaLabelText,
        string showAllSectionsText,
        string showSectionText,
        string showSectionAriaLabelText,
        IEnumerable<AccordionItem> items);

    TagBuilder GenerateBackLink(IHtmlContent content, AttributeDictionary attributes);

    TagBuilder GenerateBreadcrumbs(
        bool collapseOnMobile,
        AttributeDictionary attributes,
        IEnumerable<BreadcrumbsItem> items);

    TagBuilder GenerateButton(
        bool isStartButton,
        bool disabled,
        bool? preventDoubleClick,
        string id,
        IHtmlContent content,
        AttributeDictionary attributes);

    TagBuilder GenerateButtonLink(
        bool isStartButton,
        bool disabled,
        string id,
        IHtmlContent content,
        AttributeDictionary attributes);

    TagBuilder GenerateCharacterCount(
        string textAreaId,
        int? maxLength,
        int? maxWords,
        decimal? threshold,
        IHtmlContent formGroup,
        AttributeDictionary countMessageAttributes,
        string textAreaDescriptionText,
        (string Other, string One)? charactersUnderLimitText,
        string charactersAtLimitText,
        (string Other, string One)? charactersOverLimitText,
        (string Other, string One)? wordsUnderLimitText,
        string wordsAtLimitText,
        (string Other, string One)? wordsOverLimitText);

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
        bool? disableAutofocus,
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
        bool disabled,
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
        bool? disableAutoFocus,
        string titleId,
        int? titleHeadingLevel,
        IHtmlContent titleContent,
        IHtmlContent content,
        AttributeDictionary attributes);

    TagBuilder GeneratePagination(
        IEnumerable<PaginationItemBase> items,
        PaginationPrevious previous,
        PaginationNext next,
        string landmarkLabel,
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

    TagBuilder GenerateSummaryCard(SummaryCardTitle title, SummaryListActions actions, IHtmlContent summaryList, AttributeDictionary attributes);

    TagBuilder GenerateSummaryList(IEnumerable<SummaryListRow> rows, AttributeDictionary attributes);

    TagBuilder GenerateTabs(
        string id,
        string idPrefix,
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
        string autocapitalize,
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
