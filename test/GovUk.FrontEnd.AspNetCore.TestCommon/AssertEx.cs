using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Diffing;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Xunit.Sdk;

namespace GovUk.Frontend.AspNetCore.TestCommon
{
    public static class AssertEx
    {
        public static async Task<IDocument> GetHtmlDocument(this HttpResponseMessage response)
        {
            var html = await response.Content.ReadAsStringAsync();

            var browsingContext = BrowsingContext.New();
            return await browsingContext.OpenAsync(req => req.Content(html));
        }

        public static void HtmlEqual(
            string expected,
            string actual,
            Predicate<IDiff> excludeDiff = null,
            bool outputFullMarkupOnFailure = false)
        {
            excludeDiff ??= _ => false;

            var diffs = DiffBuilder.Compare(expected).WithTest(actual).Build()
                .Where(diff => !excludeDiff(diff) && !ExcludeDiff(diff))
                .ToArray();

            if (diffs.Any())
            {
                var sb = new StringBuilder();
                sb.AppendLine("AssertEx.HtmlEqual() Failure");
                sb.AppendLine();

                if (outputFullMarkupOnFailure)
                {
                    sb.AppendLine("Expected:");
                    sb.AppendLine(HtmlHelper.ParseHtmlElement(expected).OuterHtml);
                    sb.AppendLine();
                    sb.AppendLine("Actual:");
                    sb.AppendLine(HtmlHelper.ParseHtmlElement(actual).OuterHtml);
                }

                foreach (var diff in diffs)
                {
                    DiffConverter.Append(diff, sb);
                }

                throw new XunitException(sb.ToString());
            }

            static bool ExcludeDiff(IDiff diff)
            {
                // Handle aria-describedby being out of order
                // e.g. 'bar foo' instead of 'foo bar'
                if (diff is AttrDiff attrDiff && attrDiff.Test.Attribute.Name == "aria-describedby")
                {
                    var controlValueParts = attrDiff.Control.Attribute.Value
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    var testValueParts = attrDiff.Test.Attribute.Value
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    return controlValueParts.OrderBy(v => v, StringComparer.OrdinalIgnoreCase)
                        .SequenceEqual(testValueParts.OrderBy(v => v, StringComparer.OrdinalIgnoreCase));
                }

                return false;
            }
        }

        private static class DiffConverter
        {
            public static void Append(IDiff diff, StringBuilder builder)
            {
                if (diff is NodeDiff node)
                {
                    var received = node.Test;
                    var verified = node.Control;
                    builder.AppendLine($@" * Node Diff
   Path: {received.Path}
   Received: {received.Node.NodeValue}
   Verified: {verified.Node.NodeValue}");
                    return;
                }

                if (diff is AttrDiff attribute)
                {
                    var received = attribute.Test;
                    var verified = attribute.Control;
                    builder.AppendLine($@" * Attribute Diff
   Path: {received.Path}
   Name: {received.Attribute.Name}
   Received: {received.Attribute.Value}
   Verified: {verified.Attribute.Value}");
                    return;
                }

                if (diff is UnexpectedAttrDiff unexpectedAttribute)
                {
                    var source = unexpectedAttribute.Test;
                    builder.AppendLine($@" * Unexpected Attribute
   Path: {source.Path}
   Name: {source.Attribute.Name}
   Value: {source.Attribute.Value}");
                    return;
                }

                if (diff is UnexpectedNodeDiff unexpectedNode)
                {
                    var source = unexpectedNode.Test;
                    builder.AppendLine($@" * Unexpected Node
   Path: {source.Path}
   Name: {source.Node.NodeName}
   Value: {source.Node.NodeValue}");
                    return;
                }

                if (diff is MissingAttrDiff missingAttribute)
                {
                    var source = missingAttribute.Control;
                    builder.AppendLine($@" * Missing Attribute
   Path: {source.Path}
   Name: {source.Attribute.Name}
   Value: {source.Attribute.Value}");
                    return;
                }

                if (diff is MissingNodeDiff missingNode)
                {
                    var source = missingNode.Control;
                    builder.AppendLine($@" * Missing Node
   Path: {source.Path}
   Name: {source.Node.NodeName}
   Value: {source.Node.NodeValue}");
                    return;
                }

                throw new Exception($"Unknown diff type: {diff.GetType().FullName}.");
            }
        }
    }
}
