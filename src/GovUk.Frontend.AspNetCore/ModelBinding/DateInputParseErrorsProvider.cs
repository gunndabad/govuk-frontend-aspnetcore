using System;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.ModelBinding
{
    public sealed class DateInputParseErrorsProvider
    {
        private readonly Dictionary<string, DateInputParseErrors> _errors;

        public DateInputParseErrorsProvider()
        {
            _errors = new Dictionary<string, DateInputParseErrors>();
        }

        internal void SetErrorsForModel(string modelName, DateInputParseErrors parseErrors)
        {
            if (modelName is null)
            {
                throw new ArgumentNullException(nameof(modelName));
            }

            _errors[modelName] = parseErrors;
        }

        internal bool TryGetErrorsForModel(string modelName, out DateInputParseErrors errors)
        {
            if (modelName is null)
            {
                throw new ArgumentNullException(nameof(modelName));
            }

            return _errors.TryGetValue(modelName, out errors);
        }
    }
}
