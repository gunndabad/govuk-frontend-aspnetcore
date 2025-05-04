using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Encodings.Web;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.FileProviders;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal partial class FluidComponentGenerator : IComponentGenerator
{
    private static readonly HtmlEncoder _encoder = HtmlEncoder.Default;

    private readonly FluidParser _parser;
    private readonly ConcurrentDictionary<string, IFluidTemplate> _templates;
    private readonly TemplateOptions _templateOptions;

    public FluidComponentGenerator()
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

        _templateOptions.FileProvider = new ManifestEmbeddedFileProvider(
            typeof(GovUkFrontendAspNetCoreStartupFilter).Assembly,
            root: "ComponentGeneration/Templates");

        _templateOptions.Filters.AddFilter("nj_default", Filters.Default);
        _templateOptions.Filters.AddFilter("indent", Filters.Indent);
        _templateOptions.Filters.AddFilter("strip", Filters.Strip);

        _templateOptions.ValueConverters.Add(v =>
        {
            // If the object is an Options class, convert its property names to camel case
            if (v.GetType().Namespace?.StartsWith(GetType().Namespace!) == true)
            {
                return new ConvertNamesFromJsonTypeInfoObjectValue(v, optionsJsonSerializerOptions);
            }

            return null;
        });
    }

    public virtual IHtmlContent GenerateAccordion(AccordionOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("accordion", options);
    }

    public virtual IHtmlContent GenerateBackLink(BackLinkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("back-link", options);
    }

    public virtual IHtmlContent GenerateBreadcrumbs(BreadcrumbsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("breadcrumbs", options);
    }

    public virtual IHtmlContent GenerateButton(ButtonOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("button", options);
    }

    public virtual IHtmlContent GenerateCheckboxes(CheckboxesOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("checkboxes", options);
    }

    public virtual IHtmlContent GenerateCookieBanner(CookieBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("cookie-banner", options);
    }

    public virtual IHtmlContent GenerateErrorMessage(ErrorMessageOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("error-message", options);
    }

    public virtual IHtmlContent GenerateFieldset(FieldsetOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("fieldset", options);
    }

    public virtual IHtmlContent GenerateFileUpload(FileUploadOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("file-upload", options);
    }

    public virtual IHtmlContent GenerateHint(HintOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("hint", options);
    }

    public virtual IHtmlContent GenerateLabel(LabelOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("label", options);
    }

    public virtual IHtmlContent GenerateWarningText(WarningTextOptions options)
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

    private IHtmlContent RenderTemplate(string templateName, object componentOptions)
    {
        var context = new TemplateContext(_templateOptions);
        context.SetValue("dict", new FunctionValue(Functions.Dict));
        context.SetValue("govukAttributes", new FunctionValue(Functions.GovukAttributes));
        context.SetValue("govukI18nAttributes", new FunctionValue(Functions.GovukI18nAttributes));
        context.SetValue("ifelse", new FunctionValue(Functions.IfElse));
        context.SetValue("istruthy", new FunctionValue(Functions.IsTruthy));
        context.SetValue("params", componentOptions);  // To match the nunjucks templates

        var template = GetTemplate(templateName);
        return new HtmlString(template.Render(context, _encoder).TrimStart());
    }
}
