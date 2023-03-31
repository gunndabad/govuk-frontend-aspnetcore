using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class NotificationBannerTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_DefaultType_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-notification-banner",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-notification-banner",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The message.");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new NotificationBannerTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div
    class=""govuk-notification-banner""
    role=""region""
    aria-labelledby=""govuk-notification-banner-title""
    data-module=""govuk-notification-banner"">
    <div class=""govuk-notification-banner__header"">
        <h2 class=""govuk-notification-banner__title"" id=""govuk-notification-banner-title"">
            Important
        </h2>
    </div>
    <div class=""govuk-notification-banner__content"">
        The message.
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_SuccessType_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-notification-banner",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-notification-banner",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The message.");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new NotificationBannerTagHelper()
            {
                Type = NotificationBannerType.Success
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div
    class=""govuk-notification-banner govuk-notification-banner--success""
    role=""alert""
    aria-labelledby=""govuk-notification-banner-title""
    data-module=""govuk-notification-banner"">
    <div class=""govuk-notification-banner__header"">
        <h2 class=""govuk-notification-banner__title"" id=""govuk-notification-banner-title"">
            Success
        </h2>
    </div>
    <div class=""govuk-notification-banner__content"">
        The message.
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_WithDisableAutoFocusSpecified_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-notification-banner",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-notification-banner",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The message.");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new NotificationBannerTagHelper()
            {
                DisableAutoFocus = true,
                Type = NotificationBannerType.Success
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div
    class=""govuk-notification-banner govuk-notification-banner--success""
    role=""alert""
    aria-labelledby=""govuk-notification-banner-title""
    data-module=""govuk-notification-banner""
    data-disable-auto-focus=""true"">
    <div class=""govuk-notification-banner__header"">
        <h2 class=""govuk-notification-banner__title"" id=""govuk-notification-banner-title"">
            Success
        </h2>
    </div>
    <div class=""govuk-notification-banner__content"">
        The message.
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        // 
        [Fact]
        public async Task ProcessAsync_WithRoleSpecified_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-notification-banner",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-notification-banner",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The message.");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new NotificationBannerTagHelper()
            {
                Role = "custom-role"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div
    class=""govuk-notification-banner""
    role=""custom-role""
    aria-labelledby=""govuk-notification-banner-title""
    data-module=""govuk-notification-banner"">
    <div class=""govuk-notification-banner__header"">
        <h2 class=""govuk-notification-banner__title"" id=""govuk-notification-banner-title"">
            Important
        </h2>
    </div>
    <div class=""govuk-notification-banner__content"">
        The message.
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }

        [Fact]
        public async Task ProcessAsync_WithTitle_GeneratesExpectedOutput()
        {
            // Arrange
            var context = new TagHelperContext(
                tagName: "govuk-notification-banner",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-notification-banner",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var notificationBannerContext = context.GetContextItem<NotificationBannerContext>();
                    notificationBannerContext.SetTitle(id: "title-id", headingLevel: 4, content: new HtmlString("Title"));

                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("The message.");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new NotificationBannerTagHelper();

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            var expectedHtml = @"
<div
    class=""govuk-notification-banner""
    role=""region""
    aria-labelledby=""title-id""
    data-module=""govuk-notification-banner"">
    <div class=""govuk-notification-banner__header"">
        <h4 class=""govuk-notification-banner__title"" id=""title-id"">
            Title
        </h4>
    </div>
    <div class=""govuk-notification-banner__content"">
        The message.
    </div>
</div>";

            AssertEx.HtmlEqual(expectedHtml, output.ToHtmlString());
        }
    }
}
