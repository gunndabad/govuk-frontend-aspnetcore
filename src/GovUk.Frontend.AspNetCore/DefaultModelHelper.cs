using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#if NETSTANDARD2_0
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
#endif

namespace GovUk.Frontend.AspNetCore
{
    public class DefaultModelHelper : IModelHelper
    {
        private delegate string GetFullHtmlFieldNameDelegate(ViewContext viewContext, string expression);

        private static readonly GetFullHtmlFieldNameDelegate s_getFullHtmlFieldNameDelegate;

        static DefaultModelHelper()
        {
            s_getFullHtmlFieldNameDelegate =
#if NETSTANDARD2_0
                NameAndIdProvider.GetFullHtmlFieldName;
#else
                (GetFullHtmlFieldNameDelegate)typeof(IHtmlGenerator).Assembly
                    .GetType("Microsoft.AspNetCore.Mvc.ViewFeatures.NameAndIdProvider", throwOnError: true)
                    .GetMethod("GetFullHtmlFieldName", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .CreateDelegate(typeof(GetFullHtmlFieldNameDelegate));
#endif
        }

        public virtual string GetDisplayName(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression)
        {
            // See https://github.com/aspnet/AspNetCore/blob/master/src/Mvc/Mvc.ViewFeatures/src/DefaultHtmlGenerator.cs#L427

            var displayName = modelExplorer.Metadata.DisplayName ?? modelExplorer.Metadata.PropertyName;

            if (displayName != null && expression != null)
            {
                displayName = displayName.Split('.').Last();
            }

            return displayName;
        }

        public virtual string GetFullHtmlFieldName(ViewContext viewContext, string expression) =>
            s_getFullHtmlFieldNameDelegate(viewContext, expression);

        public virtual string GetModelValue(ViewContext viewContext, ModelExplorer modelExplorer, string expression)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException(nameof(viewContext));
            }

            if (modelExplorer == null)
            {
                throw new ArgumentNullException(nameof(modelExplorer));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var fullName = GetFullHtmlFieldName(viewContext, expression);

            // See https://github.com/dotnet/aspnetcore/blob/9a3aacb56af7221bfb29d851ee6b7c883650ddf6/src/Mvc/Mvc.ViewFeatures/src/DefaultHtmlGenerator.cs#L714-L724

            viewContext.ViewData.ModelState.TryGetValue(fullName, out var entry);

            var value = string.Empty;
            if (entry != null && entry.AttemptedValue != null)
            {
                value = entry.AttemptedValue;
            }
            else if (modelExplorer.Model != null)
            {
                value = modelExplorer.Model.ToString();
            }

            return value;
        }

        public virtual string GetValidationMessage(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string expression)
        {
            // See https://github.com/aspnet/AspNetCore/blob/master/src/Mvc/Mvc.ViewFeatures/src/DefaultHtmlGenerator.cs#L795

            var fullName = GetFullHtmlFieldName(viewContext, expression);

            if (!viewContext.ViewData.ModelState.ContainsKey(fullName))
            {
                return null;
            }

            var tryGetModelStateResult = viewContext.ViewData.ModelState.TryGetValue(fullName, out var entry);
            var modelErrors = tryGetModelStateResult ? entry.Errors : null;

            ModelError modelError = null;
            if (modelErrors != null && modelErrors.Count != 0)
            {
                modelError = modelErrors.FirstOrDefault(m => !string.IsNullOrEmpty(m.ErrorMessage)) ?? modelErrors[0];
            }

            if (modelError == null)
            {
                return null;
            }

            return modelError.ErrorMessage;
        }
    }
}
