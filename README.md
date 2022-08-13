# ASP.NET Core MVC tag helpers for GOV.UK Design System

![ci](https://github.com/gunndabad/govuk-frontend-aspnetcore/workflows/ci/badge.svg)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/GovUk.Frontend.AspNetCore)

Targets [GDS Frontend v4.1.0](https://github.com/alphagov/govuk-frontend/releases/tag/v4.1.0)

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

### 3. Register tag helpers

In your `_ViewImports.cshtml` file:

```razor:
@addTagHelper *, GovUk.Frontend.AspNetCore
```

### 4. Configure your page template

You have several options for configuring your [page template](https://design-system.service.gov.uk/styles/page-template/).

#### Using the `_GovUkPageTemplate` Razor view

A Razor view is provided with the standard page template markup and Razor sections where you can add in your header, footer and any custom markup you require.

In your `_Layout.cshtml` file:

```razor:
@{
    Layout = "_GovUkPageTemplate";
}

@section Header {
    @* your header markup goes here *@
}

@RenderBody()

@section Footer {
    @* your footer markup goes here *@
}
```

As well as `Header` and `Footer` the view defines `EndHead`, `StartBody` and `EndBody` sections.
Any custom stylesheets and scripts can be imported from within these sections.

Define `ViewBag.Title` in your views to set the `<title>` tag.

#### Create your own Razor view

If the standard template above is not sufficient, you can create your own Razor view.

By default references to the GDS frontend CSS and script assets will be added automatically to the `<head>` and `<body>` elements.

If you want to control the asset references yourself you can disable the automatic import:
```cs
services.AddGovUkFrontend(options => options.AddImportsToHtml = false);
```

The `PageTemplateHelper` class defines several methods that can simplify the CSS and script imports.
`GenerateStyleImports` imports CSS stylesheets and should be added to `<head>`.
`GenerateJsEnabledScript` declares some inline JavaScript that adds the `js-enabled` class to the `<body>` and should be placed at the start of `<body>`.
`GenerateScriptImports` imports JavaScript files and should be added to the end of `<body>`.

The latter two methods take an optional `cspNonce` parameter; when provided a `nonce` attribute will be added to the inline scripts.

`PageTemplateHelper` can be injected into your view and used like so:
```razor
@inject GovUk.Frontend.AspNetCore.PageTemplateHelper PageTemplateHelper

@PageTemplateHelper.GenerateStyleImports()
```

#### Content security policy (CSP)

There are two built-in mechanisms to help in generating a `script-src` CSP directive that works correctly with the inline scripts used by the page template.

The preferred option is to use the `GetCspScriptHashes` method on `PageTemplateHelper`. This will return a string that can be inserted directly into the `script-src` directive in your CSP.

Alternatively, a CSP nonce can be appended to the generated `script` tags. A delegate must be configured on `GovUkFrontendOptions` that retrieves a nonce for a given `HttpContext`.
```cs
services.AddGovUkFrontend(options =>
{
    options.GetCspNonceForRequest = context =>
    {
        // Return your nonce here
    };
});
```

See the `Samples.MvcStarter` project for an example of this working.


## GDS assets

This package serves the GDS Frontend assets (stylesheets, javascript, fonts) inside the host application so these do not need to be imported separately.

## Components

- [Accordion](docs/components/accordion.md)
- [Back link](docs/components/back-link.md)
- [Breadcrumbs](docs/components/breadcrumbs.md)
- [Button](docs/components/button.md)
- [Checkboxes](docs/components/checkboxes.md)
- [Character count](docs/components/character-count.md)
- [Date input](docs/components/date-input.md)
- [Details](docs/components/details.md)
- [Error message](docs/components/error-message.md)
- [Error summary](docs/components/error-summary.md)
- [Fieldset](docs/components/fieldset.md)
- [File upload](docs/components/file-upload.md)
- [Inset text](docs/components/inset-text.md)
- [Notification banner](docs/components/notification-banner.md)
- [Panel](docs/components/panel.md)
- [Phase banner](docs/components/phase-banner.md)
- [Radios](docs/components/radios.md)
- [Select](docs/components/select.md)
- [Skip link](docs/components/skip-link.md)
- [Summary list](docs/components/summary-list.md)
- [Tag](docs/components/tag.md)
- [Textarea](docs/components/textarea.md)
- [Text input](docs/components/text-input.md)
- [Warning text](docs/components/warning-text.md)

## Validators

- [Max words validator](docs/validation/maxwords.md)
