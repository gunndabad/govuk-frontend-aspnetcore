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

        _templateOptions.Filters.AddFilter("nj_default", Filters.Default);
        _templateOptions.Filters.AddFilter("indent", Filters.Indent);
        _templateOptions.Filters.AddFilter("strip", Filters.Strip);

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
        return RenderTemplate("accordion", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateBackLinkAsync(BackLinkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("back-link", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateBreadcrumbsAsync(BreadcrumbsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("breadcrumbs", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateButtonAsync(ButtonOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("button", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateCharacterCountAsync(CharacterCountOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("character-count", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateCheckboxesAsync(CheckboxesOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("checkboxes", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateCookieBannerAsync(CookieBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("cookie-banner", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateErrorMessageAsync(ErrorMessageOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("error-message", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateErrorSummaryAsync(ErrorSummaryOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options = options with { ErrorList = options.ErrorList?.ToList() };  // TEMP workaround issue with arrays
        return RenderTemplate("error-summary", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateFieldsetAsync(FieldsetOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("fieldset", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateFileUploadAsync(FileUploadOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("file-upload", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateHintAsync(HintOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("hint", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateLabelAsync(LabelOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("label", options);
    }

    public ValueTask<IHtmlContent> GenerateServiceNavigationAsync(ServiceNavigationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("service-navigation", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateTagAsync(TagOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("tag", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateTextareaAsync(TextareaOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("textarea", options);
    }

    public virtual ValueTask<IHtmlContent> GenerateWarningTextAsync(WarningTextOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("warning-text", options);
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

    private async ValueTask<IHtmlContent> RenderTemplate(string templateName, object componentOptions)
    {
        var context = new TemplateContext(_templateOptions);
        context.SetValue("dict", new FunctionValue(Functions.Dict));
        context.SetValue("govukAttributes", new FunctionValue(Functions.GovukAttributes));
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
