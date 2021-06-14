using System;

namespace GovUk.Frontend.AspNetCore
{
    [Flags]
    public enum DateInputErrorItems
    {
        None = 0,
        Day = 1,
        Month = 2,
        Year = 4,
        All = Day | Month | Year
    }
}
