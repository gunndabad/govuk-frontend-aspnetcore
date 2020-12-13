# ASP.NET Core MVC tag helpers for GOV.UK Design System

![ci](https://github.com/gunndabad/govuk-frontend-aspnetcore/workflows/ci/badge.svg)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/GovUk.Frontend.AspNetCore)

Targets [GDS Frontend v3.9.1](https://github.com/alphagov/govuk-frontend/releases/tag/v3.9.1)

## Installation

### 1. Install NuGet package

Install the [GovUk.Frontend.AspNetCore NuGet package](https://www.nuget.org/packages/GovUk.Frontend.AspNetCore/):

    Install-Package GovUk.Frontend.AspNetCore

Or via the .NET Core command line interface:

    dotnet add package GovUk.Frontend.AspNetCore

### 2. Configure your ASP.NET Core application

Add services to your application's `Startup` class:

```cs
using GovUk.Frontend.AspNetCore;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGovUkFrontend();
    }
}
```

This will register a [Tag Helper Component](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/th-components?view=aspnetcore-5.0) that automatically adds stylesheet and script imports to your Razor views.
If you do *not* want this then use the overload of `AddGovUkFrontend` that takes an `Action<GovUkFrontendAspNetCoreOptions>` parameter and set `AddImportsToHtml` to `false`:

```cs
services.AddGovUkFrontend(options =>
{
    options.AddImportsToHtml = false;
});
```

### 3. Register tag helpers

In your `_ViewImports.cshtml` file:

```razor:
@addTagHelper *, GovUk.Frontend.AspNetCore
```

## GDS assets

This package serves the GDS Frontend assets (stylesheets, javascript, fonts) inside the host application so these do not need to be imported separately.

## Components

- [x] [Accordion](docs/accordion.md)
