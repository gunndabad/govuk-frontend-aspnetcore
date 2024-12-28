using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using AngleSharp.Diffing.Core;
using GovUk.Frontend.AspNetCore.ConformanceTests.OptionsJson;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TestCommon;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit.Abstractions;

namespace GovUk.Frontend.AspNetCore.ConformanceTests;

public partial class ComponentTests
{
    private delegate IHtmlContent GenerateFormGroupElement(bool haveError, string describedBy);

    private static readonly IHtmlContent _emptyContent = new HtmlString("");

    private readonly ComponentGenerator _componentGenerator;

    private readonly string _govUkFrontendVersion;

    private readonly ITestOutputHelper _outputHelper;


    public ComponentTests(ITestOutputHelper outputHelper)
    {
        _componentGenerator = new ComponentGenerator();

        // Get GovUkFrontendVersion either from environment or from Directory.Build.props
        _govUkFrontendVersion = GetGovUkFrontendVersion();

        _outputHelper = outputHelper;
    }

    private static void AppendToDescribedBy(ref string describedBy, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (string.IsNullOrEmpty(describedBy))
        {
            describedBy = value;
        }
        else
        {
            describedBy += " " + value;
        }
    }


    private void CheckComponentHtmlMatchesExpectedHtml<TOptions>(
        ComponentTestCaseData<TOptions> testCaseData,
        Func<ComponentGenerator, TOptions, string> generateComponent,
        Predicate<IDiff> excludeDiff = null)
    {
        var html = generateComponent(_componentGenerator, testCaseData.Options);

        // Retrieve the currently executing test method and class dynamically
        var testClass = GetCurrentTestClassName();
        var testMethod = GetCurrentTestMethodName();
        var testMethodData = testCaseData.Name;
        var testMethodDisplay = $"{testClass}__{testMethod}--{testMethodData}";
        var testMethodDisplaySafe = string.Join("_", testMethodDisplay.Split(Path.GetInvalidFileNameChars()));

        // Base directory for output
        var outputBaseDirectory = Path.Combine(Path.GetTempPath(), "govuk-frontend-aspnetcore");
        var expectedFilePath = Path.Combine(outputBaseDirectory, $"{testMethodDisplaySafe}--expected--{_govUkFrontendVersion}.html");
        var actualFilePath = Path.Combine(outputBaseDirectory, $"{testMethodDisplaySafe}--actual--{_govUkFrontendVersion}.html");

        try
        {
            // Create base directory if it doesn't exist
            Directory.CreateDirectory(outputBaseDirectory);

            // Write the expected and actual HTML to files for easier diffing later
            File.WriteAllText(expectedFilePath, testCaseData.ExpectedHtml);
            File.WriteAllText(actualFilePath, html);

            _outputHelper.WriteLine($"Expected HTML written to: {expectedFilePath}");
            _outputHelper.WriteLine($"Actual HTML written to: {actualFilePath}");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to write HTML files to {outputBaseDirectory}.", ex);
        }

        // Assert that the generated HTML matches the expected HTML
        AssertEx.HtmlEqual(testCaseData.ExpectedHtml, html, excludeDiff);
    }

    private static string GetCurrentTestMethodName()
    {
        // Tests are partial extensions of the class `ComponentTests`.
        // Iterate over the stack trace to find the first method from the test class
        var stackTrace = new StackTrace();
        // foreach (var frame in stackTrace.GetFrames())
        // {
        //     var method = frame.GetMethod();
        //     if ((method?.DeclaringType != null && method.DeclaringType.IsSubclassOf(typeof(ComponentTests))) || method?.DeclaringType == typeof(ComponentTests))
        //     {
        //         return method.Name;
        //     }
        // }
        //
        // return "UnknownMethod";

        // The above code is incorrect because it will always return the name of the method that calls GetCurrentTestMethodName
        // Instead, let's just go "up" 2 frames as a hacky interim way to get the test method name
        var method = stackTrace?.GetFrame(2)?.GetMethod();

        return method?.Name ?? "UnknownMethod";
    }

    private static string GetCurrentTestClassName()
    {
        // Tests are partial extensions of the class `ComponentTests`.
        // Iterate over the stack trace to find the declaring type of the first method from the test class
        var stackTrace = new StackTrace();
        // foreach (var frame in stackTrace.GetFrames())
        // {
        //     var method = frame.GetMethod();
        //     if ((method?.DeclaringType != null && method.DeclaringType.IsSubclassOf(typeof(ComponentTests))) || method?.DeclaringType == typeof(ComponentTests))
        //     {
        //         return method.DeclaringType.Name;
        //     }
        // }

        // The above code is incorrect because it will always return the name of the method that calls GetCurrentTestClassName
        // Instead, let's just go "up" 2 frames as a hacky interim way to get the test class name
        var method = stackTrace?.GetFrame(2)?.GetMethod();
        var className = method?.DeclaringType?.Name;

        return className ?? "UnknownClass";
    }


    private static string GetGovUkFrontendVersion()
    {
        // Check for the environment variable first (easier to set in CI)
        var version = Environment.GetEnvironmentVariable("GovUkFrontendVersion");
        if (!string.IsNullOrEmpty(version))
        {
            return version;
        }

        // Fallback to reading from Directory.Build.props (via AssemblyMetadataAttribute)
        var govUkFrontendVersion = typeof(PageTemplateHelper).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                .Single(a => a.Key == "GovUkFrontendVersion")
                .Value!;

        return govUkFrontendVersion;
    }

    private string GenerateFormGroup(
        Label label,
        Hint hint,
        ErrorMessage errorMessage,
        FormGroup formGroup,
        Fieldset fieldset,
        GenerateFormGroupElement generateElement)
    {
        var haveError = errorMessage != null;

        string describedBy = null;

        var attributes = new AttributeDictionary()
            .MergeAttribute("class", formGroup?.Classes);

        var contentBuilder = new HtmlContentBuilder();

        if (label != null)
        {
            var labelTagBuilder = BuildLabel(_componentGenerator, label);
            contentBuilder.AppendHtml(labelTagBuilder);
        }

        if (hint != null)
        {
            var hintTagBuilder = BuildHint(_componentGenerator, hint);
            contentBuilder.AppendHtml(hintTagBuilder);

            AppendToDescribedBy(ref describedBy, hint.Id);
        }

        if (errorMessage != null)
        {
            var errorMessageTagBuilder = BuildErrorMessage(_componentGenerator, errorMessage);
            contentBuilder.AppendHtml(errorMessageTagBuilder);

            AppendToDescribedBy(ref describedBy, errorMessage.Id);
        }

        var element = generateElement(haveError, describedBy);
        contentBuilder.AppendHtml(element);

        IHtmlContent content = contentBuilder;

        if (fieldset != null)
        {
            AppendToDescribedBy(ref describedBy, fieldset.DescribedBy);

            content = _componentGenerator.GenerateFieldset(
                describedBy,
                role: fieldset.Role,
                fieldset.Legend?.IsPageHeading ?? ComponentGenerator.FieldsetLegendDefaultIsPageHeading,
                legendContent: TextOrHtmlHelper.GetHtmlContent(
                    fieldset.Legend?.Text,
                    fieldset.Legend?.Html),
                legendAttributes: new AttributeDictionary()
                    .MergeAttribute("class", fieldset.Legend?.Classes),
                content: contentBuilder,
                attributes: fieldset.Attributes.ToAttributesDictionary()
                    .MergeAttribute("class", fieldset.Classes));
        }

        return _componentGenerator.GenerateFormGroup(
                haveError,
                content,
                attributes)
            .ToHtmlString();
    }
}
