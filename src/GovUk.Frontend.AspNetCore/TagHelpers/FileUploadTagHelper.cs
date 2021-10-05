#nullable enable
using System.Collections.Generic;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.TagHelpers
{
    /// <summary>
    /// Generates a GDS File upload component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [RestrictChildren(LabelTagName, HintTagName, ErrorMessageTagName)]
    public class FileUploadTagHelper : FormGroupTagHelperBase
    {
        internal const string ErrorMessageTagName = "govuk-file-upload-error-message";
        internal const string HintTagName = "govuk-file-upload-hint";
        internal const string LabelTagName = "govuk-file-upload-label";
        internal const string TagName = "govuk-file-upload";

        private const string AttributesPrefix = "input-";
        private const string IdAttributeName = "id";
        private const string NameAttributeName = "name";

        /// <summary>
        /// Creates an <see cref="TextInputTagHelper"/>.
        /// </summary>
        public FileUploadTagHelper()
            : this(htmlGenerator: null, modelHelper: null)
        {
        }

        internal FileUploadTagHelper(IGovUkHtmlGenerator? htmlGenerator = null, IModelHelper? modelHelper = null)
            : base(
                  htmlGenerator ?? new ComponentGenerator(),
                  modelHelper ?? new DefaultModelHelper())
        {
        }

        /// <summary>
        /// The <c>id</c> attribute for the generated <c>input</c> element.
        /// </summary>
        /// <remarks>
        /// If not specified then a value is generated from the <c>name</c> attribute.
        /// </remarks>
        [HtmlAttributeName(IdAttributeName)]
        public string? Id { get; set; }

        /// <summary>
        /// Additional attributes to add to the generated <c>input</c> element.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = AttributesPrefix)]
        public IDictionary<string, string> InputAttributes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// The <c>name</c> attribute for the generated <c>input</c> element.
        /// </summary>
        /// <remarks>
        /// Required unless <see cref="FormGroupTagHelperBase.AspFor"/> is specified.
        /// </remarks>
        [HtmlAttributeName(NameAttributeName)]
        public string? Name { get; set; }

        private protected override FormGroupContext CreateFormGroupContext() => new FileUploadContext();

        private protected override IHtmlContent GenerateFormGroupContent(
            TagHelperContext context,
            FormGroupContext formGroupContext,
            out bool haveError)
        {
            var contentBuilder = new HtmlContentBuilder();

            var label = GenerateLabel(formGroupContext);
            contentBuilder.AppendHtml(label);

            var hint = GenerateHint(formGroupContext);
            if (hint != null)
            {
                contentBuilder.AppendHtml(hint);
            }

            var errorMessage = GenerateErrorMessage(formGroupContext);
            if (errorMessage != null)
            {
                contentBuilder.AppendHtml(errorMessage);
            }

            haveError = errorMessage != null;

            var inputTagBuilder = GenerateFileUpload(haveError);
            contentBuilder.AppendHtml(inputTagBuilder);

            return contentBuilder;

            TagBuilder GenerateFileUpload(bool haveError)
            {
                var resolvedId = ResolveIdPrefix();
                var resolvedName = ResolveName();

                return Generator.GenerateFileUpload(
                    haveError,
                    resolvedId,
                    resolvedName,
                    DescribedBy,
                    InputAttributes);
            }
        }

        private protected override string ResolveIdPrefix()
        {
            if (Id != null)
            {
                return Id;
            }

            if (Name == null && AspFor == null)
            {
                throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                    IdAttributeName,
                    NameAttributeName,
                    AspForAttributeName);
            }

            var resolvedName = ResolveName();

            return TagBuilder.CreateSanitizedId(resolvedName, Constants.IdAttributeDotReplacement);
        }

        private string ResolveName()
        {
            if (Name == null && AspFor == null)
            {
                throw ExceptionHelper.AtLeastOneOfAttributesMustBeProvided(
                    NameAttributeName,
                    AspForAttributeName);
            }

            return Name ?? ModelHelper.GetFullHtmlFieldName(ViewContext, AspFor!.Name);
        }
    }
}
