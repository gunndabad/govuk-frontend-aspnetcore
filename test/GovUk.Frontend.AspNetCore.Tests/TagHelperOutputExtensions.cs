using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Tests
{
    public static class TagHelperOutputExtensions
    {
        public static string AsString(this TagHelperOutput output)
        {
            var writer = new StringWriter();
            output.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
