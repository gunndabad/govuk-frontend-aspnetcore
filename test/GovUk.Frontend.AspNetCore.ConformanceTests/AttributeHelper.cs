using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public static class AttributeHelper
    {
        public static AttributeDictionary MergeAttribute(
            this AttributeDictionary attributes,
            string key,
            object value)
        {
            if (value == null)
            {
                return attributes;
            }

            var newValue = AttributeValueToString(value);
            string mergedValue;

            if (attributes.ContainsKey(key))
            {
                if (key == "class" || key == "aria-describedby")
                {
                    mergedValue = attributes[key] + " " + newValue;
                }
                else
                {
                    throw new InvalidOperationException($"Don't know how to merge attributes with key '{key}'.");
                }
            }
            else
            {
                mergedValue = newValue;
            }

            attributes[key] = mergedValue;

            return attributes;
        }

        public static AttributeDictionary ToAttributesDictionary(
            this IDictionary<string, object> attributes)
        {
            var attributeDictionary = new AttributeDictionary();

            if (attributes != null)
            {
                foreach (var kvp in attributes)
                {
                    attributeDictionary.Add(kvp.Key, AttributeValueToString(kvp.Value));
                }
            }

            return attributeDictionary;
        }

        private static string AttributeValueToString(object value) => value switch
        {
            bool b => b ? "true" : "false",
            _ => value.ToString()
        };
    }
}
