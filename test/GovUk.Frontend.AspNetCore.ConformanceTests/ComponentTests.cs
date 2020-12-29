using System;
using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ConformanceTests
{
    public partial class ComponentTests
    {
        private static readonly IHtmlContent _emptyContent = new HtmlString("");

        private void CheckComponentHtmlMatchesExpectedHtml<TOptions>(
            ComponentTestCaseData<TOptions> testCaseData,
            Func<ComponentGenerator, TOptions, string> generateComponent,
            Predicate<IDiff> excludeDiff = null)
        {
            var componentGenerator = new ComponentGenerator();
            var html = generateComponent(componentGenerator, testCaseData.Options);

            AssertEx.HtmlEqual(testCaseData.ExpectedHtml, html, excludeDiff);
        }
    }
}
