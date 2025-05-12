using System.Text;
using Fluid;
using Fluid.Values;

namespace GovUk.Frontend.AspNetCore.Components;

internal partial class DefaultComponentGenerator
{
    private static class Filters
    {
        public static ValueTask<FluidValue> DefaultAsync(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            // Mirror the nunjucks default filter (which looks for undefined only);
            // in our case we'll look for null.

            if (input.IsNil())
            {
                return arguments.At(0);
            }

            return input;
        }

        public static ValueTask<FluidValue> IndentAsync(FluidValue input, FilterArguments arguments, TemplateContext context)
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

        public static ValueTask<FluidValue> StripAsync(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            var encode = input is not StringValue stringValue || stringValue.Encode;
            return new StringValue(input.ToStringValue().Trim(), encode);
        }
    }
}
