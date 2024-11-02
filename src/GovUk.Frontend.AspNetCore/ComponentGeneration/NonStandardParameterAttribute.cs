using System;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

/// <summary>
/// Indicates a component parameter that is not included in the reference Nunjucks implementation.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal sealed class NonStandardParameterAttribute : Attribute { }
