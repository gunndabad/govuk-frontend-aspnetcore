using System;

namespace GovUk.Frontend.AspNetCore
{
    [Flags]
    public enum DateInputParseErrors
    {
        None = 0,
        MissingYear = 1,
        InvalidYear = 2,
        MissingMonth = 4,
        InvalidMonth = 8,
        MissingDay = 16,
        InvalidDay = 32
    }

    public static class DateInputParseErrorsExtensions
    {
        public static DateComponents GetComponentsWithErrors(this DateInputParseErrors parseErrors) =>
            DateComponents.None |
            (((parseErrors & (DateInputParseErrors.MissingDay | DateInputParseErrors.InvalidDay)) != 0) ? DateComponents.Day : 0) |
            (((parseErrors & (DateInputParseErrors.MissingMonth | DateInputParseErrors.InvalidMonth)) != 0) ? DateComponents.Month : 0) |
            (((parseErrors & (DateInputParseErrors.MissingYear | DateInputParseErrors.InvalidYear)) != 0) ? DateComponents.Year : 0);
    }
}
