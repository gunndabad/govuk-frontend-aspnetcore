using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal class FluidComponentGenerator
{
    private readonly HtmlEncoder _encoder = HtmlEncoder.Default;
    private readonly FluidParser _parser;
    private readonly ConcurrentDictionary<string, IFluidTemplate> _templates;
    private readonly TemplateOptions _templateOptions;

    public FluidComponentGenerator()
    {
        _parser = new(new FluidParserOptions { AllowFunctions = true });

        var optionsSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        };

        _templates = new();

        _templateOptions = new();
        _templateOptions.MemberAccessStrategy = new UnsafeMemberAccessStrategy();

        _templateOptions.Filters.AddFilter("nj_default", Filters.Default);
        _templateOptions.Filters.AddFilter("indent", Filters.Indent);
        _templateOptions.Filters.AddFilter("istruthy", Filters.IsTruthy);

        _templateOptions.ValueConverters.Add(v =>
        {
            if (v is IHtmlContent htmlContent)
            {
                return new StringValue(htmlContent.ToHtmlString(), encode: false);
            }

            return null;
        });

        _templateOptions.ValueConverters.Add(v =>
        {
            if (v is EncodedAttributesDictionary attrs)
            {
                return WriteAttributes(attrs);
            }

            return null;
        });

        _templateOptions.ValueConverters.Add(v =>
        {
            if (v.GetType().Namespace == typeof(FluidComponentGenerator).Namespace)
            {
                return new ConvertNamesFromJsonTypeInfoObjectValue(v, optionsSerializerOptions);
            }

            return null;
        });
    }

    public IHtmlContent GenerateAccordion(AccordionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("accordion", options);
    }

    public IHtmlContent GenerateBackLink(BackLinkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("back-link", options);
    }

    public IHtmlContent GenerateBreadcrumbs(BreadcrumbsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("breadcrumbs", options);
    }

    public IHtmlContent GenerateButton(ButtonOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("button", options);
    }

    public IHtmlContent GenerateCharacterCount(AccordionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("character-count", options);
    }

    public IHtmlContent GenerateCheckboxes(CheckboxesOptions options)
    {
        throw new NotImplementedException();
    }

    public IHtmlContent GenerateCookieBanner(CookieBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("cookie-banner", options);
    }

    public IHtmlContent GenerateDateInput(CookieBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("date-input", options);
    }

    public IHtmlContent GenerateDetails(DetailsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("details", options);
    }

    public IHtmlContent GenerateErrorMessage(ErrorMessageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("error-message", options);
    }

    public IHtmlContent GenerateErrorSummary(ErrorSummaryOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("error-summary", options);
    }

    public IHtmlContent GenerateExitThisPage(ExitThisPageOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("exit-this-page", options);
    }

    public IHtmlContent GenerateFieldset(FieldsetOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("fieldset", options);
    }

    public IHtmlContent GenerateFileUpload(FileUploadOptions options)
    {
        throw new NotImplementedException();
    }

    public IHtmlContent GenerateHint(HintOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("hint", options);
    }

    public IHtmlContent GenerateInsetText(InsetTextOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("inset-text", options);
    }

    public IHtmlContent GenerateLabel(LabelOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("label", options);
    }

    public IHtmlContent GenerateNotificationBanner(NotificationBannerOptions options)
    {
        throw new NotImplementedException();
    }

    public IHtmlContent GeneratePagination(PaginationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("pagination", options);
    }

    public IHtmlContent GeneratePanel(PanelOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("panel", options);
    }

    public IHtmlContent GeneratePhaseBanner(PhaseBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("phase-banner", options);
    }

    public IHtmlContent GenerateRadios(RadiosOptions options)
    {
        throw new NotImplementedException();
    }

    public IHtmlContent GenerateSelect(SelectOptions options)
    {
        throw new NotImplementedException();
    }

    public IHtmlContent GenerateSkipLink(SkipLinkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("skip-link", options);
    }

    public IHtmlContent GenerateSummaryList(SummaryListOptions options)
    {
        throw new NotImplementedException();
    }

    public IHtmlContent GenerateTabs(TabsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("tabs", options);
    }

    public IHtmlContent GenerateTag(TagOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("tag", options);
    }

    public IHtmlContent GenerateTaskList(TaskListOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("task-list", options);
    }

    public IHtmlContent GenerateTextarea(TextareaOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("textarea", options);
    }

    public IHtmlContent GenerateTextInput(TextInputOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("input", options);
    }

    public IHtmlContent GenerateWarningText(WarningTextOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        return RenderTemplate("warning-text", options);
    }

    private IFluidTemplate GetTemplate(string templateName) =>
        _templates.GetOrAdd(templateName, _ =>
        {
            var resourceName = $"{typeof(FluidComponentGenerator).Namespace}.Templates.{templateName}.liquid";
            using var resourceStream = typeof(FluidComponentGenerator).Assembly.GetManifestResourceStream(resourceName)
                ?? throw new ArgumentException($"Template '{templateName}' not found.", nameof(templateName));
            using var reader = new StreamReader(resourceStream);
            var source = reader.ReadToEnd();
            return _parser.Parse(source);
        });

    private IHtmlContent RenderTemplate(string templateName, object componentOptions)
    {
        var template = GetTemplate(templateName);
        var context = new TemplateContext(_templateOptions);
        context.SetValue("params", componentOptions);  // To match the nunjucks templates
        return new HtmlString(template.Render(context, _encoder).TrimStart());
    }

    private StringValue WriteAttributes(EncodedAttributesDictionary attributes)
    {
        var sb = new StringBuilder();
        using var sw = new StringWriter(sb);

        foreach (var attribute in attributes.AsTagHelperAttributes())
        {
            sb.Append(' ');
            attribute.WriteTo(sw, _encoder);
        }

        return new StringValue(sb.ToString(), encode: false);
    }

    private class ConvertNamesFromJsonTypeInfoObjectValue : ObjectValueBase
    {
        private readonly JsonTypeInfo _jsonTypeInfo;

        public ConvertNamesFromJsonTypeInfoObjectValue(object value, JsonSerializerOptions jsonSerializerOptions) : base(value)
        {
            ArgumentNullException.ThrowIfNull(jsonSerializerOptions);

            _jsonTypeInfo = jsonSerializerOptions.TypeInfoResolver!.GetTypeInfo(Value.GetType(), jsonSerializerOptions)!;
        }

        protected override FluidValue GetValue(string name, TemplateContext context)
        {
            var fixedName = GetMemberNamesFromJsonPath(name);
            return base.GetValue(fixedName, context);
        }

        public override ValueTask<FluidValue> GetValueAsync(string name, TemplateContext context)
        {
            var fixedName = GetMemberNamesFromJsonPath(name);
            return base.GetValueAsync(fixedName, context);
        }

        private string GetMemberNamesFromJsonPath(string name)
        {
            var property = _jsonTypeInfo.Properties.SingleOrDefault(p => p.Name == name);
            if (property is null)
            {
                throw new InvalidOperationException($"Cannot find property with name '{name}' on {_jsonTypeInfo.Type.Name}.");
            }

            var memberName = (property.AttributeProvider as MemberInfo)?.Name;
            if (memberName is null)
            {
                throw new InvalidOperationException($"Cannot get member name for property '{name}' on {_jsonTypeInfo.Type.Name}.");
            }

            return memberName;
        }
    }

    private static class Filters
    {
        public static ValueTask<FluidValue> Default(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            // Mirror the nunjucks default filter (which looks for undefined only);
            // in our case we'll look for null.

            if (input.IsNil())
            {
                return arguments.At(0);
            }

            return input;
        }

        public static ValueTask<FluidValue> Indent(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            var value = input.ToStringValue();
            var amount = Convert.ToInt32(arguments.At(0).ToNumberValue());
            var padding = new string(' ', amount);

            var lines = value.Split(["\r\n", "\n"], StringSplitOptions.None);

            if (lines.Length == 1)
            {
                return new StringValue(value);
            }

            var sb = new StringBuilder();
            sb.Append(lines[0]);

            foreach (var line in lines.Skip(1))
            {
                sb.AppendLine();
                sb.Append(padding);
                sb.Append(line);
            }

            return new StringValue(sb.ToString());
        }

        public static ValueTask<FluidValue> IsTruthy(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            return BooleanValue.Create(input.ToBooleanValue());
        }
    }
}
