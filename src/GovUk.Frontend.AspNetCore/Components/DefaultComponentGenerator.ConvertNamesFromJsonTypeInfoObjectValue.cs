using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Fluid;
using Fluid.Values;

namespace GovUk.Frontend.AspNetCore.Components;

internal partial class DefaultComponentGenerator
{
    private class ConvertNamesFromJsonTypeInfoObjectValue : ObjectValueBase
    {
        private readonly JsonTypeInfo _jsonTypeInfo;

        public ConvertNamesFromJsonTypeInfoObjectValue(object value, JsonSerializerOptions jsonSerializerOptions) : base(value)
        {
            ArgumentNullException.ThrowIfNull(jsonSerializerOptions);

            _jsonTypeInfo = jsonSerializerOptions.TypeInfoResolver!.GetTypeInfo(Value.GetType(), jsonSerializerOptions)!;
        }

        public override bool Contains(FluidValue value)
        {
            var valueStr = value.ToStringValue();
            var fixedName = GetMemberNamesFromJsonPath(valueStr);
            return _jsonTypeInfo.Properties.Any(p => p.Name == fixedName);
        }

        public override IEnumerable<FluidValue> Enumerate(TemplateContext context)
        {
            foreach (var property in _jsonTypeInfo.Properties.OrderBy(p => p.Order))
            {
                var key = Create(property.Name, context.Options);
                var value = Create(property.Get!(Value), context.Options);
                yield return Create(new[] { key, value }, context.Options);
            }
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
