#nullable enable
using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const bool NotificationBannerDefaultDisableAutoFocus = false;
        internal const string NotificationBannerDefaultRole = "region";
        internal const string NotificationBannerDefaultSuccessRole = "alert";
        internal const string NotificationBannerDefaultSuccessTitle = "Success";
        internal const string NotificationBannerDefaultTitle = "Important";
        internal const int NotificationBannerDefaultTitleHeadingLevel = 2;
        internal const string NotificationBannerDefaultTitleId = "govuk-notification-banner-title";
        internal const NotificationBannerType NotificationBannerDefaultType = NotificationBannerType.Default;
        internal const string NotificationBannerElement = "div";
        internal const int NotificationBannerMinHeadingLevel = 1;
        internal const int NotificationBannerMaxHeadingLevel = 6;

        public TagBuilder GenerateNotificationBanner(
            NotificationBannerType type,
            string? role,
            bool disableAutoFocus,
            string? titleId,
            int? titleHeadingLevel,
            IHtmlContent? titleContent,
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNull(nameof(content), content);

            if (titleHeadingLevel < NotificationBannerMinHeadingLevel ||
                titleHeadingLevel > NotificationBannerMaxHeadingLevel)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(titleHeadingLevel)} must be between {NotificationBannerMinHeadingLevel} and {NotificationBannerMaxHeadingLevel}.",
                    nameof(titleHeadingLevel));
            }

            role ??= type == NotificationBannerType.Success ?
                NotificationBannerDefaultSuccessRole :
                NotificationBannerDefaultRole;

            titleContent ??= new HtmlString(
                type == NotificationBannerType.Success ?
                    NotificationBannerDefaultSuccessTitle :
                    NotificationBannerDefaultTitle);

            titleId ??= NotificationBannerDefaultTitleId;

            var tagBuilder = new TagBuilder(NotificationBannerElement);
            tagBuilder.MergeOptionalAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-notification-banner");
            tagBuilder.Attributes.Add("data-module", "govuk-notification-banner");

            if (type == NotificationBannerType.Success)
            {
                tagBuilder.MergeCssClass("govuk-notification-banner--success");
            }

            tagBuilder.Attributes.Add("role", role);
            tagBuilder.Attributes.Add("aria-labelledby", titleId);

            if (disableAutoFocus)
            {
                tagBuilder.Attributes.Add("data-disable-auto-focus", "true");
            }

            tagBuilder.InnerHtml.AppendHtml(GenerateHeading());

            tagBuilder.InnerHtml.AppendHtml(GenerateContent());

            return tagBuilder;

            TagBuilder GenerateContent()
            {
                var tagBuilder = new TagBuilder("div");
                tagBuilder.MergeCssClass("govuk-notification-banner__content");
                tagBuilder.InnerHtml.AppendHtml(content);
                return tagBuilder;
            }

            TagBuilder GenerateHeading()
            {
                var tagBuilder = new TagBuilder("div");
                tagBuilder.MergeCssClass("govuk-notification-banner__header");

                var header = new TagBuilder($"h{titleHeadingLevel ?? NotificationBannerDefaultTitleHeadingLevel}");
                header.MergeCssClass("govuk-notification-banner__title");
                header.Attributes.Add("id", titleId);
                header.InnerHtml.AppendHtml(titleContent);
                tagBuilder.InnerHtml.AppendHtml(header);

                return tagBuilder;
            }
        }
    }
}
