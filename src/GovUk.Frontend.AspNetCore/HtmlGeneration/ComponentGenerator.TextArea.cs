using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TextAreaElement = "textarea";
        internal const bool TextAreaDefaultDisabled = false;
        internal const int TextAreaDefaultRows = 5;

        public virtual TagBuilder GenerateTextArea(
            bool haveError,
            string id,
            string name,
            int rows,
            string describedBy,
            string autocomplete,
            bool? spellcheck,
            bool disabled,
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNull(nameof(id), id);
            Guard.ArgumentNotNull(nameof(name), name);
            Guard.ArgumentNotNull(nameof(content), content);

            var tagBuilder = new TagBuilder(TextAreaElement);
            tagBuilder.MergeOptionalAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-textarea");

            if (haveError)
            {
                tagBuilder.MergeCssClass("govuk-textarea--error");
            }

            tagBuilder.Attributes.Add("id", id);
            tagBuilder.Attributes.Add("name", name);
            tagBuilder.Attributes.Add("rows", rows.ToString());

            if (describedBy != null)
            {
                tagBuilder.Attributes.Add("aria-describedby", describedBy);
            }

            if (autocomplete != null)
            {
                tagBuilder.Attributes.Add("autocomplete", autocomplete);
            }

            if (spellcheck.HasValue)
            {
                tagBuilder.Attributes.Add("spellcheck", spellcheck.Value ? "true" : "false");
            }

            if (disabled)
            {
                tagBuilder.Attributes.Add("disabled", "disabled");
            }

            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
