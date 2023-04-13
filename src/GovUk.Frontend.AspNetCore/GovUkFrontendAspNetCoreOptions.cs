using System;
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GovUk.Frontend.AspNetCore
{
    /// <summary>
    /// Options for configuring GovUk.Frontend.AspNetCore.
    /// </summary>
    public class GovUkFrontendAspNetCoreOptions
    {
        /// <summary>
        /// Creates a new <see cref="GovUkFrontendAspNetCoreOptions"/>.
        /// </summary>
        public GovUkFrontendAspNetCoreOptions()
        {
            AddImportsToHtml = true;

            DateInputModelConverters = new List<DateInputModelConverter>()
            {
                new DateDateInputModelConverter(),
                new DateTimeDateInputModelConverter(),
                new DateOnlyDateInputModelConverter()
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

        /// <summary>
        /// Gets a list of <see cref="DateInputModelConverter"/> used by the application.
        /// </summary>
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

        /// <summary>
        /// Creates an <see cref="IModelBinder"/> for Date input components using the <see cref="DateInputModelConverter"/>
        /// registered in <see cref="DateInputModelConverters"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="dateModelType">The type that the created <see cref="IModelBinder"/> should support.</param>
        /// <returns>
        /// A <see cref="IModelBinder"/>
        /// or <see langword="null"/> if there is no <see cref="DateInputModelConverter"/> registered that supports <paramref name="dateModelType"/>.
        /// </returns>
        public IModelBinder? GetDateInputModelBinder(Type dateModelType)
        {
            foreach (var converter in DateInputModelConverters)
            {
                if (converter.CanConvertModelType(dateModelType))
                {
                    return GetDateInputModelBinder(converter);
                }
            }

            return null;
        }

        /// <summary>
        /// Creates an <see cref="IModelBinder"/> for Date input components using the specified <see cref="DateInputModelConverter"/>.
        /// </summary>
        /// <param name="converter">The <see cref="DateInputModelConverter"/> to use to convert to and from a <see cref="Date"/>.</param>
        /// <returns>A <see cref="IModelBinder"/>.</returns>
        public IModelBinder GetDateInputModelBinder(DateInputModelConverter converter) =>
            new DateInputModelBinder(converter);
    }
}
