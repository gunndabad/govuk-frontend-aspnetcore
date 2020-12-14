#nullable enable
using System;

namespace GovUk.Frontend.AspNetCore
{
    internal static class ExceptionHelper
    {
        public static InvalidOperationException TheAttributeMustBeSpecified(string attributeName) =>
            new InvalidOperationException($"The '{attributeName}' attribute must be specified.");
    }
}
