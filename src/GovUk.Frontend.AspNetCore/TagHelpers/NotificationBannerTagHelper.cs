using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// Generates a GDS notification banner component.
/// </summary>
[HtmlTargetElement(TagName)]
[OutputElementHint(ComponentGenerator.NotificationBannerElement)]
public class NotificationBannerTagHelper : TagHelper
{
    internal const string TagName = "govuk-notification-banner";

    private const string DisableAutoFocusAttributeName = "disable-auto-focus";
    private const string RoleAttributeName = "role";
    private const string TypeAttributeName = "type";

    private readonly IGovUkHtmlGenerator _htmlGenerator;

    /// <summary>
    /// Creates a new <see cref="NotificationBannerTagHelper"/>.
    /// </summary>
    public NotificationBannerTagHelper()
        : this(htmlGenerator: null) { }

    internal NotificationBannerTagHelper(IGovUkHtmlGenerator? htmlGenerator = null)
    {
        _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
    }

    /// <summary>
    /// Whether to disable the behavior that focuses the notification banner when the page loads.
    /// </summary>
    /// <remarks>
    /// Only applies when <see cref="Type"/> is <see cref="NotificationBannerType.Success"/>.
    /// </remarks>
    [HtmlAttributeName(DisableAutoFocusAttributeName)]
    public bool? DisableAutoFocus { get; set; }

    /// <summary>
    /// The <c>role</c> attribute for the notification banner.
    /// </summary>
    /// <remarks>
    /// If <see cref="Type"/> is <see cref="NotificationBannerType.Success"/> then the default is
    /// <c>&quot;alert&quot;</c> otherwise <c>&quot;region&quot;</c>.
    /// </remarks>
    [HtmlAttributeName(RoleAttributeName)]
    public string? Role { get; set; }

    /// <summary>
    /// The type of notification.
    /// </summary>
    [HtmlAttributeName(TypeAttributeName)]
    public NotificationBannerType Type { get; set; } = ComponentGenerator.NotificationBannerDefaultType;

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var notificationBannerContext = new NotificationBannerContext();

        TagHelperContent childContent;

        using (context.SetScopedContextItem(notificationBannerContext))
        {
            childContent = await output.GetChildContentAsync();
        }

        if (output.Content.IsModified)
        {
            childContent = output.Content;
        }

        var tagBuilder = _htmlGenerator.GenerateNotificationBanner(
            Type,
            Role,
            DisableAutoFocus,
            notificationBannerContext.Title?.Id,
            notificationBannerContext.Title?.HeadingLevel,
            notificationBannerContext.Title?.Content,
            childContent.Snapshot(),
            output.Attributes.ToAttributeDictionary()
        );

        output.TagName = tagBuilder.TagName;
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.Clear();
        output.MergeAttributes(tagBuilder);
        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
    }
}
