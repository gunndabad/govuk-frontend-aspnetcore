using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

internal static class TestUtils
{
    public static ViewContext CreateViewContext() =>
        new ViewContext() { HttpContext = new DefaultHttpContext() };
}
