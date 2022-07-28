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

            PrependErrorSummaryToForms = true;
            PrependErrorToTitle = true;
        }

        /// <summary>
        /// Whether <c>style</c> and <c>script</c> tags to import GDS assets are added to <c>head</c> and <c>body</c>
        /// elements automatically in Razor views.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        public bool AddImportsToHtml { get; set; }

        public List<DateInputModelConverter> DateInputModelConverters { get; }

        /// <summary>
        /// Whether to prepend an error summary component to forms.
        /// </summary>
        /// <remarks>
        /// This can be overriden on a form-by-form basis by setting the <c>gfa-prepend-error-summary</c> attribute.
        /// </remarks>
        public bool PrependErrorSummaryToForms { get; set; }

        /// <summary>
        /// Whether to prepend 'Error: ' to the &lt;title&gt; element when ModelState is not valid.
        /// </summary>
        /// <remarks>
        /// The default is <see langword="true"/>.
        /// </remarks>
        public bool PrependErrorToTitle { get; set; }
    }
}
