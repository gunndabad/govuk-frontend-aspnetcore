#nullable enable
using System;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Represents the title in a GDS notification banner component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = NotificationBannerTagHelper.TagName)]
    public class NotificationBannerTitleTagHelper : TagHelper
    {
        internal const string TagName = "govuk-notification-banner-title";

        private const string HeadingLevelAttributeName = "heading-level";
        private const string IdAttributeName = "heading-level";

        private int _headingLevel = ComponentGenerator.NotificationBannerDefaultTitleHeadingLevel;
        private string _id = ComponentGenerator.NotificationBannerDefaultTitleId;

        /// <summary>
        /// Creates a new <see cref="NotificationBannerTitleTagHelper"/>.
        /// </summary>
        public NotificationBannerTitleTagHelper()
        {
        }

        /// <summary>
        /// The heading level for the notification banner title.
        /// </summary>
        /// <remarks>
        /// Must be between <c>1</c> and <c>6</c> (inclusive). The default is <c>2</c>.
        /// </remarks>
        [HtmlAttributeName(HeadingLevelAttributeName)]
        public int HeadingLevel
        {
            get => _headingLevel;
            set
            {
                if (value < ComponentGenerator.NotificationBannerMinHeadingLevel ||
                    value > ComponentGenerator.NotificationBannerMaxHeadingLevel)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        $"{nameof(HeadingLevel)} must be between {ComponentGenerator.NotificationBannerMinHeadingLevel} and {ComponentGenerator.NotificationBannerMaxHeadingLevel}.");
                }

                _headingLevel = value;
            }
        }

        /// <summary>
        /// The <c>id</c> attribute for the notification banner title.
        /// </summary>
        /// <remarks>
        /// The default is <c>&quot;govuk-notification-banner-title&quot;</c>.
        /// Cannot be <c>null</c> or empty.
        /// </remarks>
        [HtmlAttributeName(IdAttributeName)]
        public string Id
        {
            get => _id;
            set => _id = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var notificationBannerContext = context.GetContextItem<NotificationBannerContext>();

            var childContent = output.TagMode == TagMode.StartTagAndEndTag ?
                await output.GetChildContentAsync() :
                null;

            notificationBannerContext.SetTitle(Id, HeadingLevel, childContent?.Snapshot());

            output.SuppressOutput();
        }
    }
}
