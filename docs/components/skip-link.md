# Skip link

[GDS Skip link component](https://design-system.service.gov.uk/components/skip-link/)

## Example - default href

```razor
<govuk-skip-link>Skip to main content</govuk-skip-link>
```

## Example - custom href

```razor
<govuk-skip-link href="#main">Skip to main content</govuk-skip-link>
```

## API

### `<govuk-skip-link>`

The content is the HTML to use within the generated link.

| Attribute | Type     | Description                                                                              |
|-----------|----------|------------------------------------------------------------------------------------------|
| `href`    | `string` | The 'href' attribute for the link. The default is '#content'. Cannot be `null` or empty. |
