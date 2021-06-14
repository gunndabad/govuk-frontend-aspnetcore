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
                new DateDateInputModelConverter()
            };
        }

        public bool AddImportsToHtml { get; set; }

        public List<DateInputModelConverter> DateInputModelConverters { get; }
    }
}
