# Changelog

## Unreleased

Targets GOV.UK Frontend v5.4.1.### Tag helper changes

#### `<govuk-breadcrumbs>` tag helper
A `label-text` attribute has been added.

## 2.3.0

Targets GOV.UK Frontend v5.3.1 and .NET 8.

### New features

#### `DateInputAttribute`
This attribute can be added to properties that are model bound from date input components. It allows overriding the prefix used for error messages e.g.
```cs
[DateInput(ErrorMessagePrefix = "Your date of birth")]
public DateOnly? DateOfBirth { get; set; }
```

### Tag helper changes

#### `<govuk-input>` tag helper
An `autocapitalize` attribute has been added.
Attributes can be set on the input wrapper element by specifying `input-wrapper-*` attributes.

### Fixes

#### Page template
Fix duplicate `PathBase` in OpengraphImageUrl in page template view.

## 2.2.0

Targets GOV.UK Frontend v5.2.0.

## 2.1.0

#### Page template

The `StaticAssetsContentPath` and `CompiledContentPath` properties on `GovUkFrontendOptions` have been changed from `string` to `PathString?`.

The `GenerateScriptImports`, `GenerateStyleImports` and `GetCspScriptHashes` methods on `PageTemplateHelper` and the corresponding extension methods over `IHtmlHelper`
have had overloads added that take a `PathString pathBase` parameter.

The `_GovUkPageTemplate.cshtml` view has been fixed to respect `HttpRequest.PathBase`.

Middleware has been added to rewrite the URL references in `all.min.css` to respect `HttpRequest.PathBase` and the `StaticAssetsContentPath`.

## 2.0.1

#### Page template

New overloads of `GenerateScriptImports` and `GenerateStyleImports` have been added that accept an `appendVersion` parameter.
This appends a query string with a hash of the file's contents so that content changes following upgrades are seen by end users.

A `GetCspScriptHashes` extension method on `IHtmlHelper` has been added that forwards to the same method on `PageTemplateHelper`.

## 2.0.0

Targets GOV.UK Frontend v5.1.0.

### New features

#### GOV.UK Frontend hosting options

Previously the GOV.UK Frontend library's assets were always hosted at the root of the application.
Many applications generate their own CSS and/or JavaScript bundles and don't need the standard versions at all, though they likely still need the static assets (fonts, images etc.).
There are now two properties on `GovUkFrontendOptions` to control the hosting of the static assets and the compiled assets - `StaticAssetsContentPath` (default `/assets`) and `CompiledContentPath` (default `/govuk`), respectively.
Applications that build and reference their own CSS and JavaScript can set `CompiledContentPath` to `null` to skip hosting the standard compiled assets. Similarly, setting `StaticAssetsContentPath` to `null` will skip hosting the static assets.

#### Page template

`PageTemplateHelper` and the `_GovUkPageTemplate.cshtml` view have been updated to respect the `StaticAssetsContentPath` and `CompiledContentPath` paths set on `GovUkFrontendOptions`.

An additional `ViewData` key can now be passed to `_GovUkPageTemplate.cshtml` - `AssetPath`. When specified, it will be used in place of the `StaticAssetsContentPath` value from `GovUkFrontendOptions` for referencing static asserts.

`GovUkFrontendJsEnabledScript`, `GovUkFrontendScriptImports` and `GovUkFrontendStyleImports` extension methods have been added over `IHtmlHelper` that wrap the
`GenerateJsEnabledScript`, `GovUkFrontendScriptImports` and `GovUkFrontendStyleImports` methods on `PageTemplateHelper`, respectively.

### Tag helper changes

`bool` tag helper properties have been changed to `bool?`.
This is so that it's possible to differentiate between properties that have been explicitly initialized and those that have been left at the default values.
With this, other tag helpers or tag helper initializers can be created that assign default values to these properties.

### Breaking changes

#### `AddImportsToHtml`

This option was used to automatically add style and JavaScript imports to all Razor views.
`PageTemplateHelper` and the `_GovUkPageTemplate.cshtml` layout view are better ways to generate a full page template now so this option, along with the backing tag helper component, have been removed.

### Fixes

#### Page template
The `og:image` `meta` tag in the `_GovUkPageTemplate.cshtml` view is now an absolute URL.
