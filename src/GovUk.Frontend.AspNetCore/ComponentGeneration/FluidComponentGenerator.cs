using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
using Microsoft.Extensions.FileProviders;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal partial class FluidComponentGenerator : IComponentGenerator2
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

    public virtual string GenerateAccordion(AccordionOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("accordion", options);
    }

    public virtual string GenerateBackLink(BackLinkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("back-link", options);
    }

    public virtual string GenerateBreadcrumbs(BreadcrumbsOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("breadcrumbs", options);
    }

    public virtual string GenerateButton(ButtonOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("button", options);
    }

    public virtual string GenerateCheckboxes(CheckboxesOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("checkboxes", options);
    }

    public virtual string GenerateCookieBanner(CookieBannerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("cookie-banner", options);
    }

    public virtual string GenerateErrorMessage(ErrorMessageOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("error-message", options);
    }

    public virtual string GenerateFieldset(FieldsetOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("fieldset", options);
    }

    public virtual string GenerateFileUpload(FileUploadOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("file-upload", options);
    }

    public virtual string GenerateHint(HintOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("hint", options);
    }

    public virtual string GenerateLabel(LabelOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("label", options);
    }

    public virtual string GenerateWarningText(WarningTextOptions options)
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

    private string RenderTemplate(string templateName, object componentOptions)
    {
        var context = new TemplateContext(_templateOptions);
        context.SetValue("dict", new FunctionValue(Functions.Dict));
        context.SetValue("govukAttributes", new FunctionValue(Functions.GovukAttributes));
        context.SetValue("govukI18nAttributes", new FunctionValue(Functions.GovukI18nAttributes));
        context.SetValue("ifelse", new FunctionValue(Functions.IfElse));
        context.SetValue("istruthy", new FunctionValue(Functions.IsTruthy));
        context.SetValue("params", componentOptions);  // To match the nunjucks templates

        var template = GetTemplate(templateName);
        return template.Render(context, _encoder).TrimStart();
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
            var indentFirstLine = arguments["indent_first"]?.ToBooleanValue() ?? false;

            var encode = input is not StringValue stringValue || stringValue.Encode;

            var value = input.ToStringValue();
            var amount = Convert.ToInt32(arguments.At(0).ToNumberValue());
            var padding = new string(' ', amount);

            var lines = value.Split(["\r\n", "\n"], StringSplitOptions.None);

            if (lines.Length == 1 && !indentFirstLine)
            {
                return new StringValue(value, encode);
            }

            var sb = new StringBuilder();

            if (indentFirstLine)
            {
                sb.Append(padding);
            }
            sb.Append(lines[0]);

            foreach (var line in lines.Skip(1))
            {
                sb.AppendLine();
                sb.Append(padding);
                sb.Append(line);
            }

            return new StringValue(sb.ToString(), encode);
        }

        public static ValueTask<FluidValue> Strip(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            var encode = input is not StringValue stringValue || stringValue.Encode;
            return new StringValue(input.ToStringValue().Trim(), encode);
        }
    }

    private static class Functions
    {
        public static FluidValue Dict(FunctionArguments args, TemplateContext context)
        {
            var result = new Dictionary<string, FluidValue>();

            foreach (var name in args.Names)
            {
                var value = args[name]!;
                result.Add(name, value);
            }

            return FluidValue.Create(result, context.Options);
        }

        public static FluidValue GovukAttributes(FunctionArguments args, TemplateContext context)
        {
            // https://github.com/alphagov/govuk-frontend/blob/v5.8.0/packages/govuk-frontend/src/govuk/macros/attributes.njk

            var attrsArg = args.At(0);
            string attributesHtml;

            if (attrsArg.Type == FluidValues.String)
            {
                attributesHtml = attrsArg.ToStringValue();
            }
            else if (attrsArg.Type is FluidValues.Object && attrsArg.ToObjectValue() is AttributeCollection attributeCollection)
            {
                var sb = new StringBuilder();

                foreach (var attribute in attributeCollection.GetAttributes())
                {
                    sb.Append(' ');
                    sb.Append(_encoder.Encode(attribute.Name));

                    if (!attribute.Optional || attribute.Value is not true)
                    {
                        sb.Append('=');
                        sb.Append('"');
                        sb.Append(_encoder.Encode(attribute.Value?.ToString() ?? string.Empty));
                        sb.Append('"');
                    }
                }

                attributesHtml = sb.ToString();
            }
            else if (attrsArg.Type == FluidValues.Nil)
            {
                attributesHtml = string.Empty;
            }
            else
            {
                throw new InvalidOperationException($"Cannot convert {attrsArg.Type} to attributes.");
            }

            return new StringValue(attributesHtml, encode: false);
        }

        public static FluidValue GovukI18nAttributes(FunctionArguments args, TemplateContext context)
        {
            var message = args["message"];

            if (!message.IsNil())
            {
                var attr = $" data-i18n.{args["key"].ToStringValue()}=\"{_encoder.Encode(args["message"].ToStringValue())}\"";
                return new StringValue(attr, encode: false);
            }

            return NilValue.Instance;
        }

        public static FluidValue IfElse(FunctionArguments args, TemplateContext context)
        {
            return args.At(0).ToBooleanValue() ? args.At(1) : args.At(2);
        }

        public static FluidValue IsTruthy(FunctionArguments args, TemplateContext context)
        {
            return FluidValue.Create(args.At(0).ToBooleanValue(), context.Options);
        }
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
}
