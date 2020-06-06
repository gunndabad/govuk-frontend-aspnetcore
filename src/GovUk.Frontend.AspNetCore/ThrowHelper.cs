using System;

namespace GovUk.Frontend.AspNetCore
{
    internal static class ThrowHelper
    {
        internal static void AtLeastOneOfAttributesMustBeSpecified(string attribute1, string attribute2) =>
            throw new InvalidOperationException(
                $"At least one of the '{attribute1}' and '{attribute2}' attributes must be specified.");

        internal static void OnlyOneElementAllowed(string tagName) =>
            throw new InvalidOperationException($"Only one <{tagName}> can be specified.");
    }
}
