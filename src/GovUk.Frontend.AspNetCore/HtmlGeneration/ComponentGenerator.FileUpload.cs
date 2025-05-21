using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const string FileUploadElement = "input";
    internal const bool FileUploadDefaultDisabled = false;

    public TagBuilder GenerateFileUpload(
        bool haveError,
        string? id,
        string name,
        string describedBy,
        bool disabled,
        AttributeDictionary attributes)
    {
        throw new NotImplementedException();
    }
}
