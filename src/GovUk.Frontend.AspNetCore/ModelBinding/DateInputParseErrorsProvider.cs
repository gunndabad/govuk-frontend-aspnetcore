#nullable enable
using System;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ModelBinding
{
    /// <summary>
    /// Contains the set of date input parse errors for a request.
    /// </summary>
    public class DateInputParseErrorsProvider
    {
        private readonly Dictionary<string, DateInputParseErrors> _errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateInputParseErrorsProvider"/> class.
        /// </summary>
        public DateInputParseErrorsProvider()
        {
            _errors = new Dictionary<string, DateInputParseErrors>();
        }

        /// <summary>
        /// Sets the date input parse errors for a model name.
        /// </summary>
        /// <param name="modelName">The model name.</param>
        /// <param name="parseErrors">The <see cref="DateInputParseErrors"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="modelName"/> argument is null.</exception>
        public void SetErrorsForModel(string modelName, DateInputParseErrors parseErrors)
        {
            if (modelName is null)
            {
                throw new ArgumentNullException(nameof(modelName));
            }

            _errors[modelName] = parseErrors;
        }

        /// <summary>
        /// Gets the <see cref="DateInputParseErrors"/> for a model name.
        /// </summary>
        /// <param name="modelName">The model name.</param>
        /// <param name="errors">The <see cref="DateInputParseErrors"/>.</param>
        /// <returns><see langword="true"/> if errors have been set for the specified model name; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="modelName"/> argument is null.</exception>
        public bool TryGetErrorsForModel(string modelName, out DateInputParseErrors errors)
        {
            if (modelName is null)
            {
                throw new ArgumentNullException(nameof(modelName));
            }

            return _errors.TryGetValue(modelName, out errors);
        }
    }
}
