using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    [HtmlTargetElement("govuk-file-upload")]
    [RestrictChildren("govuk-file-upload-label", "govuk-file-upload-hint", "govuk-file-upload-error-message")]
    public class FileUploadTagHelper : FormGroupTagHelperBase
    {
        private const string AttributesPrefix = "input-";
        private const string IdAttributeName = "id";

        public FileUploadTagHelper(IGovUkHtmlGenerator htmlGenerator)
            : base(htmlGenerator)
        {
        }

        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        protected override TagBuilder GenerateElement(TagHelperContext context, FormGroupBuilder builder, FormGroupElementContext elementContext)
        {
            return Generator.GenerateFileUpload(
                elementContext.HaveError,
                ResolvedId,
                ResolvedName,
                DescribedBy,
                Attributes);
        }

        protected override string GetIdPrefix() => Id;
    }

    [HtmlTargetElement("govuk-file-upload-label", ParentTag = "govuk-file-upload")]
    public class FileUploadLabelTagHelper : FormGroupLabelTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-file-upload-hint", ParentTag = "govuk-file-upload")]
    public class FileUploadHintTagHelper : FormGroupHintTagHelperBase
    {
    }

    [HtmlTargetElement("govuk-file-upload-error-message", ParentTag = "govuk-file-upload")]
    public class FileUploadErrorMessageTagHelper : FormGroupErrorMessageTagHelperBase
    {
    }
}
