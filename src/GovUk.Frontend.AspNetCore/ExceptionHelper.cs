#nullable enable
using System;
using System.Linq;

namespace GovUk.Frontend.AspNetCore
{
    internal static class ExceptionHelper
    {
        public static InvalidOperationException AChildElementMustBeProvided(string childElementTagName) =>
            new InvalidOperationException($"A <{childElementTagName}> element must be provided.");

        public static InvalidOperationException AtLeastOneOfAttributesMustBeProvided(params string[] attributeNames)
        {
            if (attributeNames.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(attributeNames));
            }

            var quotedNames = attributeNames.Select(a => $"'{a}'").ToArray();

            var attrsList = string.Join(", ", quotedNames.SkipLast(2).Append(string.Join(" and ", quotedNames.TakeLast(2))));

            return new InvalidOperationException($"At least one of the {attrsList} attributes must be provided.");
        }

        public static InvalidOperationException ChildElementMustBeSpecifiedBefore(
            string childElementTagName, string beforeSiblingTagName) =>
                throw new InvalidOperationException($"<{childElementTagName}> must be specified before <{beforeSiblingTagName}>.");

        public static InvalidOperationException OnlyOneElementIsPermittedIn(
            string childElementTagName, string parentElementTagName) =>
                new InvalidOperationException($"Only one <{childElementTagName}> element is permitted within each <{parentElementTagName}>.");

        public static InvalidOperationException TheAttributeMustBeSpecified(string attributeName) =>
            new InvalidOperationException($"The '{attributeName}' attribute must be specified.");
    }
}
