#nullable enable
using System;
using System.Text.RegularExpressions;

namespace GovUk.Frontend.AspNetCore.Validation
{
    /// <summary>
    /// Helper class to validate strings are no larger than a specified number of words.
    /// </summary>
    public class MaxWordsValidator
    {
        // Must match https://github.com/alphagov/govuk-frontend/blob/v3.14.0/src/govuk/components/character-count/character-count.js#L91
        private static readonly Regex _pattern = new Regex(@"\S+", RegexOptions.Compiled | RegexOptions.Multiline);

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxWordsValidator"/> class.
        /// </summary>
        /// <param name="maxWords">The maximum allowable number of words</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="maxWords"/> argument is not a positive integer.</exception>
        public MaxWordsValidator(int maxWords)
        {
            if (maxWords <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWords), "maxWords must be a positive integer.");
            }

            MaxWords = maxWords;
        }

        /// <summary>
        /// Gets the maximum allowable number of words.
        /// </summary>
        public int MaxWords { get; }

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A value indicating whether validation was successful.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> argument is null.</exception>
        public bool IsValid(string? value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var wordCount = _pattern.Matches(value).Count;
            return wordCount <= MaxWords;
        }
    }
}
