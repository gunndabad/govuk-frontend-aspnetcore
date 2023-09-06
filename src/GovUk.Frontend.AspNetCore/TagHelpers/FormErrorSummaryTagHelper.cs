using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.AspNetCore.TagHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting &lt;form&gt; elements that prepends a GDS error summary component to the form.
/// </summary>
[HtmlTargetElement("form")]
public class FormErrorSummaryTagHelper : TagHelper
{
    private const string PrependErrorSummaryAttributeName = "gfa-prepend-error-summary";

    private readonly GovUkFrontendAspNetCoreOptions _options;
    private readonly IGovUkHtmlGenerator _htmlGenerator;

    /// <summary>
    /// Creates a <see cref="FormErrorSummaryTagHelper"/>.
    /// </summary>
    public FormErrorSummaryTagHelper(IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor)
        : this(optionsAccessor, htmlGenerator: null)
    {
    }

    internal FormErrorSummaryTagHelper(
        IOptions<GovUkFrontendAspNetCoreOptions> optionsAccessor,
        IGovUkHtmlGenerator? htmlGenerator)
    {
        _options = Guard.ArgumentNotNull(nameof(optionsAccessor), optionsAccessor).Value;
        _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
    }

    /// <summary>
    /// Whether to prepend an error summary component to this form.
    /// </summary>
    /// <remarks>
    /// The default is set for the application in <see cref="GovUkFrontendAspNetCoreOptions.PrependErrorSummaryToForms"/>.
    /// </remarks>
    [HtmlAttributeName(PrependErrorSummaryAttributeName)]
    public bool? PrependErrorSummary { get; set; }

    /// <inheritdoc/>
    public override void Init(TagHelperContext context)
    {
        // N.B. We deliberately do not use SetScopedContextItem here; nested forms are not supported
        context.Items.Add(typeof(FormErrorContext), new FormErrorContext());
    }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await output.GetChildContentAsync();

        var prependErrorSummary = PrependErrorSummary ?? _options.PrependErrorSummaryToForms;
        if (!prependErrorSummary)
        {
            return;
        }

        var formErrorContext = (FormErrorContext)context.Items[typeof(FormErrorContext)];
        if (formErrorContext.Errors.Count == 0)
        {
            return;
        }

        var errorItems = formErrorContext.Errors.Select(i => new ErrorSummaryItem()
        {
            Content = i.Content,
            Href = i.Href
        });

        var errorSummary = _htmlGenerator.GenerateErrorSummary(
            disableAutofocus: null,  // TODO Should we have an attribute to configure this?
            titleContent: new HtmlString(HtmlEncoder.Default.Encode(ComponentGenerator.ErrorSummaryDefaultTitle)),
            titleAttributes: null,
            descriptionContent: null,
            descriptionAttributes: null,
            attributes: null,
            items: errorItems);

        output.PreContent.AppendHtml(errorSummary);
    }
}
