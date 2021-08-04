#nullable enable
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string FileUploadElement = "input";

        public TagBuilder GenerateFileUpload(
            bool haveError,
            string id,
            string name,
            string describedBy,
            IDictionary<string, string> attributes)
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

            if (describedBy != null)
            {
                tagBuilder.Attributes.Add("aria-describedby", describedBy);
            }

            return tagBuilder;
        }
    }
}
