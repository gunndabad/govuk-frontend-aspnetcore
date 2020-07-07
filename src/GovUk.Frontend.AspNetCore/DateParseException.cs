using System;

namespace GovUk.Frontend.AspNetCore
{
    public class DateParseException : Exception
    {
        internal DateParseException(string message, DateParseErrorComponents errorComponents)
            : base(message)
        {
            ErrorComponents = errorComponents;
        }

        public DateParseErrorComponents ErrorComponents { get; }
    }
}
