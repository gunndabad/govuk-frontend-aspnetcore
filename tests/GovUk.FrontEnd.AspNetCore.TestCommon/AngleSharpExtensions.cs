using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace GovUk.Frontend.AspNetCore.TestCommon;

public static class AngleSharpExtensions
{
    public static IReadOnlyList<IElement> GetAllElementsByTestId(this IElement element, string testId) =>
        element.QuerySelectorAll($"*[data-testid='{testId}']").ToList().AsReadOnly();

    public static IElement? GetElementByTestId(this IElement element, string testId) =>
        element.GetAllElementsByTestId(testId).SingleOrDefault();

    public static IReadOnlyList<IElement> GetAllElementsByTestId(this IHtmlDocument doc, string testId) =>
        doc.Body!.GetAllElementsByTestId(testId);

    public static IElement? GetElementByTestId(this IHtmlDocument doc, string testId) =>
        doc.Body!.GetElementByTestId(testId);
}
