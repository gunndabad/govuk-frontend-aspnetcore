using System;

namespace GovUk.Frontend.AspNetCore
{
    [Flags]
    public enum DateComponents
    {
        None = 0,
        Day = 1,
        Month = 2,
        Year = 4,
    }
}
