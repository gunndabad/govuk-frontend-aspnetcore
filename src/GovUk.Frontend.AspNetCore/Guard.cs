#nullable enable
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace GovUk.Frontend.AspNetCore
{
    internal static class Guard
    {
        public static T ArgumentNotNull<T>(string argName, [NotNull] T? argValue)
            where T : class
        {
            if (argValue == null)
            {
                throw new ArgumentNullException(argName);
            }

            return argValue;
        }

        public static T ArgumentNotNull<T>(
            string argName,
            string message,
            [NotNull] T? testValue)
            where T : struct
        {
            if (testValue == null)
            {
                throw new ArgumentException(message, argName);
            }

            return testValue.Value;
        }

        public static string ArgumentNotNullOrEmpty(string argName, [NotNull] string? argValue)
        {
            if (argValue == null)
            {
                throw new ArgumentNullException(argName);
            }

            if (string.IsNullOrEmpty(argValue))
            {
                throw new ArgumentException("Argument was empty.", argName);
            }

            return argValue;
        }

        public static T ArgumentNotNullOrEmpty<T>(string argName, [NotNull] T? argValue)
            where T : class, IEnumerable
        {
            ArgumentNotNull(argName, argValue);

            if (!argValue.GetEnumerator().MoveNext())
            {
                throw new ArgumentException("Argument was empty.", argName);
            }

            return argValue;
        }

        public static void ArgumentValid(
            string argName,
            string message,
            bool test)
        {
            if (!test)
            {
                throw new ArgumentException(message, argName);
            }
        }

        public static T ArgumentValidNotNull<T>(
            string argName,
            string message,
            [NotNull] T? testValue,
            bool test)
            where T : class
        {
            if (testValue == null || !test)
            {
                throw new ArgumentException(message, argName);
            }

            return testValue;
        }
    }
}
