#nullable enable

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    internal class FileUploadContext : FormGroupContext
    {
        protected override string ErrorMessageTagName => FileUploadTagHelper.ErrorMessageTagName;

        protected override string HintTagName => FileUploadTagHelper.HintTagName;

        protected override string LabelTagName => FileUploadTagHelper.LabelTagName;

        protected override string RootTagName => FileUploadTagHelper.TagName;
    }
}
