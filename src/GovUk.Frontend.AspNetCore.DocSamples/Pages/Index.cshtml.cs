using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

namespace GovUk.Frontend.AspNetCore.DocSamples.Pages;

public class IndexModel : PageModel
{
    private readonly IEnumerable<EndpointDataSource> _endpointDataSources;

    public IEnumerable<SampleDetail> SampleDetails { get; private set; }

    public List<string> ComponentNames() => SampleDetails
        .Select(x => x.ComponentName)
        .Distinct()
        .OrderBy(x => x)
        .ToList();

    public IndexModel(IEnumerable<EndpointDataSource> endpointDataSources) =>
        _endpointDataSources = endpointDataSources;

    public void OnGet()
    {
        // Assign and filter RouteEndpoints.
        var endpointSources = _endpointDataSources
            .SelectMany(source => source.Endpoints)
            .OfType<RouteEndpoint>()
            .ToList();

        Console.Out.WriteLine($"Found {endpointSources.Count} RouteEndpoints.");

        // Assign and filter SampleDetails.
        SampleDetails = endpointSources
            .Select(SampleDetail.FromRouteEndpoint)
            .Where(detail => detail is not null)
            .Cast<SampleDetail>()
            .OrderBy(detail => detail.ComponentName)
            .ThenBy(detail => detail.SampleName)
            .ToList();
    }

    public static string MakeReadable(string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? string.Empty
            : Regex.Replace(input, "(\\B[A-Z])", " $1")
                .ToLower()
                .Trim();

    public record SampleDetail(
        string ComponentName,
        string SampleName,
        string RoutePattern,
        string DisplayName,
        string? CodeSnippet = null)
    {
        public static SampleDetail? FromRouteEndpoint(RouteEndpoint endpoint)
        {
            if (!IsRouteForComponent(endpoint))
            {
                return null;
            }

            var routeParts = GetRoutePartsFromEndpoint(endpoint);
            if (routeParts.Count != 3)
            {
                return null;
            }

            var componentName = routeParts[1];
            var sampleName = routeParts[2];

            var filePath = $"Pages/GovUkFrontendComponent/{componentName}/{sampleName}.cshtml";

            return new SampleDetail(
                ComponentName: componentName,
                SampleName: sampleName,
                RoutePattern: endpoint.RoutePattern.RawText,
                DisplayName: endpoint.DisplayName,
                CodeSnippet: System.IO.File.ReadAllText(filePath)
            );
        }


        private static bool IsRouteForComponent(RouteEndpoint endpoint)
        {
            var routePattern = endpoint.DisplayName;
            if (routePattern is null)
            {
                Console.Error.WriteLine("Route is null.");
                return false;
            }

            var routePatternParts = GetRoutePartsFromEndpoint(endpoint);
            if (routePatternParts.Count != 3)
            {
                Console.Error.WriteLine($"Route '{routePattern}' does not have 4 parts.");
                return false;
            }

            if (routePatternParts[0] != "GovUkFrontendComponent")
            {
                Console.Error.WriteLine($"Route '{routePattern}' does not start with '/GovUkFrontendComponent'.");
                return false;
            }

            return true;
        }

        private static List<string> GetRoutePartsFromEndpoint(RouteEndpoint endpoint)
        {
            var segments = endpoint.RoutePattern.PathSegments;
            var routePatternParts = segments
                .Select(segment => segment.Parts)
                .SelectMany(parts => parts)
                .Select(part => part switch
                {
                    RoutePatternLiteralPart literalPart => literalPart.Content,
                    RoutePatternParameterPart parameterPart => parameterPart.Name,
                    _ => string.Empty
                })
                .ToList();

            return routePatternParts;
        }
    }
}
