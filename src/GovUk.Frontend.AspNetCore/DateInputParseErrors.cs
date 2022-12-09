using System;

namespace GovUk.Frontend.AspNetCore
{
    /// <summary>
    /// The errors that occurred when parsing the components of a date input component.
    /// </summary>
    [Flags]
    public enum DateInputParseErrors
    {
        /// <summary>
        /// No errors.
        /// </summary>
        None = 0,

        /// <summary>
        /// The year component is missing.
        /// </summary>
        MissingYear = 1,

        /// <summary>
        /// The year component is invalid.
        /// </summary>
        InvalidYear = 2,

        /// <summary>
        /// The month component is missing.
        /// </summary>
        MissingMonth = 4,

        /// <summary>
        /// The month component is invalid.
        /// </summary>
        InvalidMonth = 8,

        /// <summary>
        /// The day component is missing.
        /// </summary>
        MissingDay = 16,

        /// <summary>
        /// The day component is invalid.
        /// </summary>
        InvalidDay = 32
    }

    /// <summary>
    /// Provides extension methods for working with <see cref="DateInputParseErrors"/>.
    /// </summary>
    public static class DateInputParseErrorsExtensions
    {
        /// <summary>
        /// Groups the specified <see cref="DateInputParseErrors"/> by the affected date input components.
        /// </summary>
        public static DateInputErrorComponents GetErrorComponents(this DateInputParseErrors parseErrors) =>
            DateInputErrorComponents.None |
            (((parseErrors & (DateInputParseErrors.MissingDay | DateInputParseErrors.InvalidDay)) != 0) ? DateInputErrorComponents.Day : 0) |
            (((parseErrors & (DateInputParseErrors.MissingMonth | DateInputParseErrors.InvalidMonth)) != 0) ? DateInputErrorComponents.Month : 0) |
            (((parseErrors & (DateInputParseErrors.MissingYear | DateInputParseErrors.InvalidYear)) != 0) ? DateInputErrorComponents.Year : 0);
    }
}
