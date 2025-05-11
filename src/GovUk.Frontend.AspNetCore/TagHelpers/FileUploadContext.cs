namespace GovUk.Frontend.AspNetCore.TagHelpers;

internal class FileUploadContext : FormGroupContext3
{
    protected override IReadOnlyCollection<string> ErrorMessageTagNames { get; } = [FileUploadTagHelper.ErrorMessageTagName];

    protected override IReadOnlyCollection<string> HintTagNames { get; } = [FileUploadTagHelper.HintTagName];

    protected override IReadOnlyCollection<string> LabelTagNames { get; } = [FileUploadTagHelper.LabelTagName];

    protected override string RootTagName => FileUploadTagHelper.TagName;
}
