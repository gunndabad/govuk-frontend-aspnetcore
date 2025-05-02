using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Fluid;
using Fluid.Ast;
using Fluid.Values;
using Microsoft.Extensions.FileProviders;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal class FluidComponentGenerator
{
    private static readonly HtmlEncoder _encoder = HtmlEncoder.Default;

    private readonly FluidParser _parser;
    private readonly ConcurrentDictionary<string, IFluidTemplate> _templates;
    private readonly TemplateOptions _templateOptions;
    private readonly JsonSerializerOptions _optionsJsonSerializerOptions;

    public FluidComponentGenerator()
    {
        _parser = new FluidParser(new FluidParserOptions()
        {
            AllowFunctions = true,
            AllowParentheses = true
        });

        _optionsJsonSerializerOptions = ComponentOptionsJsonSerializerOptions.Instance;

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
            if (v is JsonElement jsonElement)
            {
                var dictionary = jsonElement.EnumerateObject().ToDictionary(p => p.Name, p => CreateValueFromJson(p.Value));
                return FluidValue.Create(dictionary, _templateOptions);
            }

            return null;

            object? CreateValueFromJson(JsonElement element) => element.ValueKind switch
            {
                JsonValueKind.Object => element.EnumerateObject().ToDictionary(p => p.Name, p => CreateValueFromJson(p.Value)),
                JsonValueKind.Array => element.EnumerateArray().Select(CreateValueFromJson),
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.GetDecimal(),  // REVIEW
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => throw new ArgumentException($"Unrecognized {nameof(JsonValueKind)}: '{element.ValueKind}'.", nameof(element))
            };
        });
    }

    public string GenerateAccordion(AccordionOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("accordion", options);
    }

    public string GenerateBackLink(BackLinkOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("back-link", options);
    }

    public string GenerateBreadcrumbs(BreadcrumbsOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("breadcrumbs", options);
    }

    public string GenerateErrorMessage(ErrorMessageOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("error-message", options);
    }

    public string GenerateFileUpload(FileUploadOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("file-upload", options);
    }

    public string GenerateHint(HintOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("hint", options);
    }

    public string GenerateLabel(LabelOptions2 options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return RenderTemplate("label", options);
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
        var componentParams = JsonSerializer.SerializeToElement(componentOptions, _optionsJsonSerializerOptions);
        context.SetValue("params", componentParams);  // To match the nunjucks templates

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

            return DictionaryValue.Create(result, context.Options);
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
            else if (attrsArg.Type is FluidValues.Dictionary)
            {
                var sb = new StringBuilder();

                foreach (ArrayValue kvp in attrsArg.Enumerate(context))
                {
                    var optional = false;
                    FluidValue? attributeValue = null;
                    var elements = kvp.Enumerate(context).GetEnumerator();

                    elements.MoveNext();
                    var attributeName = elements.Current.ToStringValue();

                    elements.MoveNext();
                    var attributeValueFluidValue = elements.Current;

                    if (attributeValueFluidValue.Type == FluidValues.Object)
                    {
                        // TODO Unpack value and optional
                        throw new NotImplementedException();
                    }
                    else
                    {
                        attributeValue = attributeValueFluidValue;
                    }

                    sb.Append(' ');
                    sb.Append(_encoder.Encode(attributeName));

                    if (!optional && !attributeValue.Equals(BooleanValue.True))
                    {
                        sb.Append('=');
                        sb.Append('"');
                        sb.Append(_encoder.Encode(attributeValue.ToStringValue()));
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
    }
}
