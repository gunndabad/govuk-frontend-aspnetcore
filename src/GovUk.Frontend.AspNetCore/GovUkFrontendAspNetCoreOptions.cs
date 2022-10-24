#nullable enable
using System;
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Http;

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
        /// The default is <c>true</c>.
        /// </remarks>
        public bool AddImportsToHtml { get; set; }

        /// <summary>
        /// The default value for <see cref="ButtonTagHelper.PreventDoubleClick"/>.
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        public bool DefaultButtonPreventDoubleClick { get; set; } = ComponentGenerator.ButtonDefaultPreventDoubleClick;

        /// <summary>
        /// A delegate for retrieving a CSP nonce for the current request.
        /// </summary>
        /// <remarks>
        /// This is invoked when the page template utilities generate style and script import tags to add a <c>nonce</c> attribute.
        /// </remarks>
        public Func<HttpContext, string?>? GetCspNonceForRequest { get; set; }

        public List<DateInputModelConverter> DateInputModelConverters { get; }

        /// <summary>
        /// Whether to prepend an error summary component to forms.
        /// </summary>
        /// <remarks>
        /// <para>This can be overriden on a form-by-form basis by setting the <c>gfa-prepend-error-summary</c> attribute.</para>
        /// <para>The default is <c>true</c>.</para>
        /// </remarks>
        public bool PrependErrorSummaryToForms { get; set; }

        /// <summary>
        /// Whether to prepend 'Error: ' to the &lt;title&gt; element when ModelState is not valid.
        /// </summary>
        /// <remarks>
        /// The default is <c>true</c>.
        /// </remarks>
        public bool PrependErrorToTitle { get; set; }
    }
}
