﻿namespace GovUk.Frontend.AspNetCore
{
    public static class HtmlSnippets
    {
        public const string GdsLibraryVersion = "3.9.1";

        public const string BodyInitScript = @"<script>
    document.body.className = document.body.className + ' js-enabled';
</script>";

        public const string ScriptImports = @"<script src=""/govuk-frontend-3.9.1.min.js""></script>
<script>window.GOVUKFrontend.initAll()</script>";

        public const string StyleImports = @"<!--[if !IE 8]><!-->
    <link rel=""stylesheet"" href=""/govuk-frontend-3.9.1.min.css"">
<!--<![endif]-->
<!--[if IE 8]>
    <link rel = ""stylesheet"" href=""/govuk-frontend-ie8-3.9.1.min.css"">
<![endif]-->";
    }
}
