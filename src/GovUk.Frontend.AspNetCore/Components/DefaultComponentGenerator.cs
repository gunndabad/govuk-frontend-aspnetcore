using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.FileProviders;

namespace GovUk.Frontend.AspNetCore.Components;

internal partial class DefaultComponentGenerator : IComponentGenerator
{
    internal const string DefaultErrorSummaryTitleHtml = "There is a problem";

    private static readonly HtmlEncoder _encoder = HtmlEncoder.Default;

    private readonly FluidParser _parser;
    private readonly ConcurrentDictionary<string, IFluidTemplate> _templates;
    private readonly TemplateOptions _templateOptions;

    public DefaultComponentGenerator()
    {
        _parser = new FluidParser(new FluidParserOptions()
        {
            AllowFunctions = true,
            AllowParentheses = true
        });

        var optionsJsonSerializerOptions = ComponentOptionsJsonSerializerOptions.Instance;

        _templates = new ConcurrentDictionary<string, IFluidTemplate>();

        _templateOptions = new TemplateOptions();
        _templateOptions.MemberAccessStrategy = new UnsafeMemberAccessStrategy();
        _templateOptions.Trimming = TrimmingFlags.TagLeft;

        _templateOptions.FileProvider = new ManifestEmbeddedFileProvider(
            typeof(GovUkFrontendAspNetCoreStartupFilter).Assembly,
            root: "Components/Templates");

        _templateOptions.Filters.AddFilter("nj_default", Filters.DefaultAsync);
        _templateOptions.Filters.AddFilter("indent", Filters.IndentAsync);
        _templateOptions.Filters.AddFilter("strip", Filters.StripAsync);

        _templateOptions.ValueConverters.Add(v =>
        {
            if (v is TemplateString templateString)
            {
                return templateString.ToFluidValue();
            }

            // If the object is an Options class, convert its property names to camel case
            if (v.GetType().Namespace?.StartsWith(GetType().Namespace!) == true)
            {
                return new ConvertNamesFromJsonTypeInfoObjectValue(v, optionsJsonSerializerOptions);
            }

            return null;
        });
    }

    public virtual ValueTask<IHtmlContent> GenerateAccordionAsync(AccordionOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("accordion", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateBackLinkAsync(BackLinkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("back-link", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateBreadcrumbsAsync(BreadcrumbsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("breadcrumbs", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateButtonAsync(ButtonOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("button", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateCharacterCountAsync(CharacterCountOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("character-count", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateCheckboxesAsync(CheckboxesOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("checkboxes", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateCookieBannerAsync(CookieBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("cookie-banner", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateDetailsAsync(DetailsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("details", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateErrorMessageAsync(ErrorMessageOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("error-message", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateErrorSummaryAsync(ErrorSummaryOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options = options with { ErrorList = options.ErrorList?.ToList() };  // TEMP workaround issue with arrays
        return RenderTemplateAsync("error-summary", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateExitThisPageAsync(ExitThisPageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("exit-this-page", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateFieldsetAsync(FieldsetOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("fieldset", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateFileUploadAsync(FileUploadOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("file-upload", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateFooterAsync(FooterOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("footer", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateHeaderAsync(HeaderOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("header", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateHintAsync(HintOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("hint", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateInsetTextAsync(InsetTextOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("inset-text", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateLabelAsync(LabelOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("label", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateServiceNavigationAsync(ServiceNavigationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("service-navigation", options);
    }

    public virtual ValueTask<IHtmlContent> GeneratePaginationAsync(PaginationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options = options with { Items = options.Items?.ToList() };  // TEMP workaround issue with arrays
        return RenderTemplateAsync("pagination", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateSummaryListAsync(SummaryListOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("summary-list", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateTagAsync(TagOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("tag", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateTextareaAsync(TextareaOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("textarea", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateWarningTextAsync(WarningTextOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplateAsync("warning-text", options);
    }

    private IFluidTemplate GetTemplate(string templateName) =>
        _templates.GetOrAdd(templateName, _ =>
        {
            var templateFileInfo = _templateOptions.FileProvider.GetFileInfo($"{templateName}.liquid");
            if (!templateFileInfo.Exists)
            {
                throw new ArgumentException($"Template '{templateName}' not found.", nameof(templateName));
            }

            using var sourceStream = templateFileInfo.CreateReadStream();
            using var reader = new StreamReader(sourceStream);
            var source = reader.ReadToEnd();

            var template = _parser.Parse(source);

            return template;
        });

    private async ValueTask<IHtmlContent> RenderTemplateAsync(string templateName, object componentOptions)
    {
        var context = new TemplateContext(_templateOptions);
        context.SetValue("dict", new FunctionValue(Functions.Dict));
        context.SetValue("govukAttributes", new FunctionValue(Functions.GovukAttributesAsync));
        context.SetValue("govukI18nAttributes", new FunctionValue(Functions.GovukI18nAttributes));
        context.SetValue("ifelse", new FunctionValue(Functions.IfElse));
        context.SetValue("istruthy", new FunctionValue(Functions.IsTruthy));
        context.SetValue("not", new FunctionValue(Functions.Not));
        context.SetValue("params", componentOptions);  // To match the nunjucks templates

        var template = GetTemplate(templateName);
        var result = await template.RenderAsync(context, _encoder);
        return new HtmlString(result.TrimStart());
    }
}
