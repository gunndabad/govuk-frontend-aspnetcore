using System;

namespace GovUk.Frontend.AspNetCore
{
    [Flags]
    public enum DateInputErrorItems
    {
        Day = 1,
        Month = 2,
        Year = 4,
        All = Day | Month | Year
    }
}
