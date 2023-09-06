using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal const string FileUploadElement = "input";

    public TagBuilder GenerateFileUpload(
        bool haveError,
        string id,
        string name,
        string describedBy,
        AttributeDictionary attributes)
    {
        Guard.ArgumentNotNull(nameof(id), id);
        Guard.ArgumentNotNull(nameof(name), name);

        var tagBuilder = new TagBuilder(FileUploadElement);
        tagBuilder.MergeAttributes(attributes);
        tagBuilder.MergeCssClass("govuk-file-upload");

        if (haveError)
        {
            tagBuilder.MergeCssClass("govuk-file-upload--error");
        }

        tagBuilder.Attributes.Add("id", id);
        tagBuilder.Attributes.Add("name", name);
        tagBuilder.Attributes.Add("type", "file");

        if (!string.IsNullOrEmpty(describedBy))
        {
            tagBuilder.Attributes.Add("aria-describedby", describedBy);
        }

        return tagBuilder;
    }
}
