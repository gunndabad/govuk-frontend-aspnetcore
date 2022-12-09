using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore
{
    internal interface IModelHelper
    {
        string? GetDescription(ModelExplorer modelExplorer);

        string? GetDisplayName(ViewContext viewContext, ModelExplorer modelExplorer, string expression);

        string GetFullHtmlFieldName(ViewContext viewContext, string expression);

        string? GetModelValue(ViewContext viewContext, ModelExplorer modelExplorer, string expression);

        string? GetValidationMessage(ViewContext viewContext, ModelExplorer modelExplorer, string expression);
    }
}
