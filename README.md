# ASP.NET Core integration for GOV.UK Design System

![ci](https://github.com/gunndabad/govuk-frontend-aspnetcore/workflows/ci/badge.svg)
![NuGet Downloads](https://img.shields.io/nuget/dt/GovUk.Frontend.AspNetCore)

Targets [GDS Frontend v5.2.0](https://github.com/alphagov/govuk-frontend/releases/tag/v5.2.0)

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

The view can be customised by defining the following sections and `ViewData`/`ViewBag` variables.

| Section name | Description |
| --- | --- |
| BeforeContent | Add content that needs to appear outside <main> element. <br /> For example: The [back link](docs/components/back-link.md) component, [breadcrumbs](docs/components/breadcrumbs.md) component, [phase banner](docs/components/phase-banner.md) component. |
| BodyEnd | Add content just before the closing `</body>` element. |
| BodyStart | Add content after the opening `<body>` element. <br/> For example: The cookie banner component. |
| Footer | Override the default footer component. |
| Head | Add additional items inside the <head> element. <br /> For example: `<meta name="description" content="My page description">` |
| Header | Override the default header component. |
| HeadIcons | Override the default icons used for GOV.UK branded pages. <br /> For example: `<link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />` |
| SkipLink | Override the default [skip link](docs/components/skip-link.md) component. |

| `ViewData` key | Type | Description |
| --- | --- | --- |
| BodyClasses | `string` | Add class(es) to the `<body>` element. |
| ContainerClasses | `string` | Add class(es) to the container. This is useful if you want to make the page wrapper a fixed width. |
| HtmlClasses | `string` | Add class(es) to the `<html>` element. |
| HtmlLang | `string` | Set the language of the whole document. If your `<title>` and `<main>` element are in a different language to the rest of the page, use `HtmlLang` to set the language of the rest of the page. |
| MainClasses | `string` | Add class(es) to the `<main>` element. |
| MainLang | `string` | Set the language of the `<main>` element if it's different to `HtmlLang`. |
| OpengraphImageUrl | `string` | Set the URL for the Open Graph image meta tag. The URL must be absolute, including the protocol and domain name. |
| Title | `string` | Override the default page title (`<title>` element). |
| ThemeColor | `string` | Set the toolbar [colour on some devices](https://developers.google.com/web/updates/2014/11/Support-for-theme-color-in-Chrome-39-for-Android). |

#### Create your own Razor view

If the standard template above is not sufficient, you can create your own Razor view.

Extension methods are provided on `IHtmlHelper` that simplify the CSS and script imports.
`GovUkFrontendStyleImports` imports CSS stylesheets and should be added to `<head>`.
`GovUkFrontendJsEnabledScript` declares some inline JavaScript that adds the `js-enabled` class to the `<body>` and should be placed at the start of `<body>`.
`GovUkFrontendScriptImports` imports JavaScript files and should be added to the end of `<body>`.

The latter two methods take an optional `cspNonce` parameter; when provided a `nonce` attribute will be added to the inline scripts.

Example `_Layout.cshtml` snippet:
```razor
@using GovUk.Frontend.AspNetCore

<!DOCTYPE html>
<html>
<head>
    @Html.GovUkFrontendStyleImports()
</head>
<body>
    @Html.GovUkFrontendJsEnabledScript()

    @RenderBody()

    @Html.GovUkFrontendScriptImports()
</body>
</html>
```

#### Content security policy (CSP)

There are two built-in mechanisms to help in generating a `script-src` CSP directive that works correctly with the inline scripts used by the page template.

The preferred option is to use the `GetCspScriptHashes` extension method on `IHtmlHelper`. This will return a string that can be inserted directly into the `script-src` directive in your CSP.

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
- [Pagination](docs/components/pagination.md)
- [Panel](docs/components/panel.md)
- [Phase banner](docs/components/phase-banner.md)
- [Radios](docs/components/radios.md)
- [Select](docs/components/select.md)
- [Skip link](docs/components/skip-link.md)
- [Summary list](docs/components/summary-list.md)
- [Tabs](docs/components/tabs.md)
- [Tag](docs/components/tag.md)
- [Textarea](docs/components/textarea.md)
- [Text input](docs/components/text-input.md)
- [Warning text](docs/components/warning-text.md)

## Validators

- [Max words validator](docs/validation/maxwords.md)
