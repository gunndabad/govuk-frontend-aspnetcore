namespace GovUk.Frontend.AspNetCore
{
    public static class HtmlSnippets
    {
        public const string ScriptImports = @"<script src=""/govuk-frontend-3.5.0.min.js""></script>
<script>window.GOVUKFrontend.initAll()</script>";

        public const string StyleImports = @"<!--[if !IE 8]><!-->
    <link rel=""stylesheet"" href=""/govuk-frontend-3.5.0.min.css"">
<!--<![endif]-->
<!--[if IE 8]>
    <link rel = ""stylesheet"" href=""/govuk-frontend-ie8-3.5.0.min.css"">
<![endif]-->";
    }
}
