#nullable enable
using System;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.AspNetCore.Validation
{
    /// <summary>
    /// Specifies the maximum number of words allowed in a property.
    /// </summary>
    public sealed class MaxWordsAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validation attribute to assert a string property does not exceed a maximum number of words.
        /// </summary>
        /// <param name="words">The maximum allowable number of words.</param>
        public MaxWordsAttribute(int words)
        {
            Words = words;
        }

        /// <summary>
        /// Gets the maximum allowable number of words.
        /// </summary>
        public int Words { get; }

        /// <inheritdoc/>
        public override bool IsValid(object? value)
        {
            if (Words < 0)
            {
                throw new InvalidOperationException("The maximum length must be a positive integer.");
            }

            var validator = new MaxWordsValidator(Words);
            return validator.IsValid((string?)value);
        }
    }
}
