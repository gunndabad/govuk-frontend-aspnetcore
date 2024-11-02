using System;

namespace GovUk.Frontend.AspNetCore.ModelBinding;

/// <summary>
/// The errors that occurred when parsing the fields of a date input component.
/// </summary>
[Flags]
public enum DateInputParseErrors
{
    /// <summary>
    /// No errors.
    /// </summary>
    None = 0,

    /// <summary>
    /// The year field is missing.
    /// </summary>
    MissingYear = 1,

    /// <summary>
    /// The year field is invalid.
    /// </summary>
    InvalidYear = 2,

    /// <summary>
    /// The month field is missing.
    /// </summary>
    MissingMonth = 4,

    /// <summary>
    /// The month field is invalid.
    /// </summary>
    InvalidMonth = 8,

    /// <summary>
    /// The day field is missing.
    /// </summary>
    MissingDay = 16,

    /// <summary>
    /// The day field is invalid.
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
    public static DateInputErrorFields GetErrorComponents(this DateInputParseErrors parseErrors) =>
        DateInputErrorFields.None |
        ((parseErrors & (DateInputParseErrors.MissingDay | DateInputParseErrors.InvalidDay)) != 0 ? DateInputErrorFields.Day : 0) |
        ((parseErrors & (DateInputParseErrors.MissingMonth | DateInputParseErrors.InvalidMonth)) != 0 ? DateInputErrorFields.Month : 0) |
        ((parseErrors & (DateInputParseErrors.MissingYear | DateInputParseErrors.InvalidYear)) != 0 ? DateInputErrorFields.Year : 0);
}
