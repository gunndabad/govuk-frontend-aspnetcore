namespace GovUk.Frontend.AspNetCore
{
    public static class HtmlSnippets
    {
        public const string GdsLibraryVersion = "4.0.0";

        public const string BodyInitScript = @"<script>
    document.body.className = document.body.className + ' js-enabled';
</script>";

        public const string ScriptImports = @"<script src=""/govuk-frontend-4.0.0.min.js""></script>
<script>window.GOVUKFrontend.initAll()</script>";

        public const string StyleImports = @"<!--[if !IE 8]><!-->
    <link rel=""stylesheet"" href=""/govuk-frontend-4.0.0.min.css"">
<!--<![endif]-->
<!--[if IE 8]>
    <link rel = ""stylesheet"" href=""/govuk-frontend-ie8-4.0.0.min.css"">
<![endif]-->";
    }
}
