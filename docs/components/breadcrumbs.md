# Breadcrumbs

[GDS Breadcrumbs component](https://design-system.service.gov.uk/components/breadcrumbs/)

## Example

```razor
<govuk-breadcrumbs collapse-on-mobile="true">
    <item asp-controller="Home" asp-action="Index">Home</item>
    <item href="#" link-target="_blank">Passports, travel and living abroad</item>
    <item>Travel abroad</item>
</govuk-breadcrumbs>
```

![Breadcrumbs](../images/breadcrumbs.png)

## API

### `<govuk-breadcrumbs>`

| Attribute | Type    | Description                                                                                                                                      |
| --- |---------|--------------------------------------------------------------------------------------------------------------------------------------------------|
| `collapse-on-mobile` | `bool?` | When true, the breadcrumbs will collapse to the first and last item only on tablet breakpoint and below. If not specified, `false` will be used. |

### `<item>`

Content is the HTML to use within the breadcrumbs item.\
Must be inside a `<govuk-breadcrumbs>` element.

| Attribute | Type | Description |
| --- | --- | --- |
| (link attributes) | | See [documentation on links](../links.md) for more information |
| `link-*` | | Additional attributes to be applied to the item's hyperlink. |
