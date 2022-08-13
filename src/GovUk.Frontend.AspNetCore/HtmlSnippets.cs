using System;

namespace GovUk.Frontend.AspNetCore
{
    public static class HtmlSnippets
    {
        public static string GdsLibraryVersion { get; } = "4.1.0";

        /// <summary>
        /// The initialization javascript for the GDS page template.
        /// </summary>
        /// <remarks>
        /// The contents of this property should be inserted at the beginning of the <c>body</c> tag.
        /// </remarks>
        [Obsolete("Use the GenerateJsEnabledScript() method on PageTemplateHelper instead.")]
        public const string BodyInitScript = @"<script>
    document.body.className = document.body.className + ' js-enabled';
</script>";

        /// <summary>
        /// HTML that imports the GOV.UK Frontend library script and initializes it.
        /// </summary>
        /// <remarks>
        /// The contents of this property should be inserted at the end of the <c>body</c> tag.
        /// </remarks>
        [Obsolete("Use the GenerateScriptImports() method on PageTemplateHelper instead.")]
        public const string ScriptImports = @"<script src=""/govuk-frontend-4.1.0.min.js""></script>
<script>window.GOVUKFrontend.initAll()</script>";

        /// <summary>
        /// The HTML that imports the GOV.UK Frontend library styles.
        /// </summary>
        /// <remarks>
        /// The contents of this property should be inserted in the <c>head</c> tag.
        /// </remarks>
        [Obsolete("Use the GenerateStyleImports() method on PageTemplateHelper instead.")]
        public const string StyleImports = @"<!--[if !IE 8]><!-->
    <link rel=""stylesheet"" href=""/govuk-frontend-4.1.0.min.css"">
<!--<![endif]-->
<!--[if IE 8]>
    <link rel = ""stylesheet"" href=""/govuk-frontend-ie8-4.1.0.min.css"">
<![endif]-->";
    }
}
