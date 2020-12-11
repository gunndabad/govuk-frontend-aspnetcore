using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore
{
    public class GovUkFrontendAspNetCoreOptions
    {
        public GovUkFrontendAspNetCoreOptions()
        {
            AddImportsToHtml = true;

            DateInputModelConverters = new List<DateInputModelConverter>()
            {
                new DateDateInputModelConverter(),
                new DateTimeDateInputModelConverter()
            };
        }

        public bool AddImportsToHtml { get; set; }

        public List<DateInputModelConverter> DateInputModelConverters { get; }
		
        internal bool RunningConformanceTests { get; set; }
    }
}
