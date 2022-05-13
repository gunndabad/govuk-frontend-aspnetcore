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

            PrependErrorToTitle = true;
        }

        public bool AddImportsToHtml { get; set; }

        public List<DateInputModelConverter> DateInputModelConverters { get; }

        /// <summary>
        /// Whether to prepend 'Error: ' to the &lt;title&gt; element when ModelState is not valid.
        /// </summary>
        /// <remarks>
        /// The default is <see langword="true"/>.
        /// </remarks>
        public bool PrependErrorToTitle { get; set; }
    }
}
