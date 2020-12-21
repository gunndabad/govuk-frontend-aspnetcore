#nullable enable
using System;

namespace GovUk.Frontend.AspNetCore
{
    internal static class ExceptionHelper
    {
        public static InvalidOperationException AChildElementMustBeProvided(string childElementTagName) =>
            new InvalidOperationException($"A <{childElementTagName}> element must be provided.");

        public static InvalidOperationException AtLeastOneOfAttributesMustBeProvided(string attr1, string attr2) =>
            new InvalidOperationException($"At least one of the '{attr1}' and '{attr2}' attributes must be provided.");

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
