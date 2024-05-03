using System;
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Http;

namespace GovUk.Frontend.AspNetCore;

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
        AcceptMonthNamesInDateInputs = true;
        AddNovalidateAttributeToForms = true;

        DateInputModelConverters = new List<DateInputModelConverter>()
        {
            new DateTimeDateInputModelConverter(),
            new DateOnlyDateInputModelConverter()
        };

        PrependErrorSummaryToForms = true;
        PrependErrorToTitle = true;
        StaticAssetsContentPath = "/assets";
        CompiledContentPath = "/govuk";
    }

    /// <summary>
    /// Whether to accept full and abbreviated month names in Date Input components.
    /// </summary>
    /// <remarks>
    /// The default is <c>true</c>.
    /// </remarks>
    public bool AcceptMonthNamesInDateInputs { get; set; }

    /// <summary>
    /// Whether to add a <c>novalidate</c> attribute to <c>form</c> elements.
    /// </summary>
    /// <remarks>
    /// The default is <c>true</c>.
    /// </remarks>
    public bool AddNovalidateAttributeToForms { get; set; }

    /// <summary>
    /// The path to serve GOV.UK Frontend static assets at.
    /// </summary>
    /// <remarks>
    /// <para>If this is <c>null</c> the static assets will not be served.</para>
    /// <para>The default is <c>/assets</c>.</para>
    /// </remarks>
    public string? StaticAssetsContentPath { get; set; }

    /// <summary>
    /// The path to serve GOV.UK Frontend compiled JavaScript and CSS at.
    /// </summary>
    /// <remarks>
    /// <para>If this is <c>null</c> the compiled assets will not be served.</para>
    /// <para>The default is <c>/govuk</c>.</para>
    /// </remarks>
    public string? CompiledContentPath { get; set; }

    /// <summary>
    /// The default value for <see cref="ButtonTagHelper.PreventDoubleClick"/>.
    /// </summary>
    public bool? DefaultButtonPreventDoubleClick { get; set; }

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
    /// Whether to prepend &quot;Error: &quot; to the &lt;title&gt; element when ModelState is not valid.
    /// </summary>
    /// <remarks>
    /// The default is <c>true</c>.
    /// </remarks>
    public bool PrependErrorToTitle { get; set; }
}
