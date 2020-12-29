using System.Collections.Generic;
using System.Linq;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public static class DictionaryExtensions
    {
        public static IDictionary<string, string> MergeAttribute(
            this IDictionary<string, string> attributes,
            string key,
            object value)
        {
            if (value == null)
            {
                return attributes;
            }

            attributes[key] = AttributeValueToString(value);

            return attributes;
        }

        public static IDictionary<string, string> ToAttributesDictionary(
            this IDictionary<string, object> attributes)
        {
            return (attributes?.ToDictionary(a => a.Key, a => AttributeValueToString(a.Value))).OrEmpty();
        }

        private static string AttributeValueToString(object value) => value switch
        {
            bool b => b ? "true" : "false",
            _ => value.ToString()
        };
    }
}
