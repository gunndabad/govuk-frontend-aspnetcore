using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers
{
    public class NotificationBannerTitleTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_SetsTitleOnContext()
        {
            // Arrange
            var notificationBannerContext = new NotificationBannerContext();

            var context = new TagHelperContext(
                tagName: "govuk-notification-banner-title",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(NotificationBannerContext), notificationBannerContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-notification-banner-title",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Title");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var tagHelper = new NotificationBannerTitleTagHelper()
            {
                HeadingLevel = 3,
                Id = "my-title"
            };

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.NotNull(notificationBannerContext.Title);
            Assert.Equal("Title", notificationBannerContext.Title?.Content?.ToHtmlString());
            Assert.Equal(3, notificationBannerContext.Title?.HeadingLevel);
            Assert.Equal("my-title", notificationBannerContext.Title?.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(7)]
        public void SetHeadingLevel_InvalidLevel_ThrowsArgumentException(int level)
        {
            // Arrange
            var notificationBannerContext = new NotificationBannerContext();

            var context = new TagHelperContext(
                tagName: "govuk-notification-banner-title",
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>()
                {
                    { typeof(NotificationBannerContext), notificationBannerContext }
                },
                uniqueId: "test");

            var output = new TagHelperOutput(
                "govuk-notification-banner-title",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Title");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            // Act
            var ex = Record.Exception(() => new NotificationBannerTitleTagHelper()
            {
                HeadingLevel = level
            });

            // Assert
            var argumentEx = Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal("value", argumentEx.ParamName);
            Assert.StartsWith("HeadingLevel must be between 1 and 6.", argumentEx.Message);
        }
    }
}
