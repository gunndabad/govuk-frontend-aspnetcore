using System;

namespace GovUk.Frontend.AspNetCore
{
    public class DateParseException : Exception
    {
        internal DateParseException(string message)
            : base(message)
        {
        }
    }
}
