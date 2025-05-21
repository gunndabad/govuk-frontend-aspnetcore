# Details

[GDS Warning text component](https://design-system.service.gov.uk/components/warning-text/)

## Example

```razor
<govuk-warning-text icon-fallback-text="Warning">
    You can be fined up to £5,000 if you do not register.
</govuk-warning-text>
```

![Warning text](../images/warning-text.png)

## API

### `<govuk-warning-text>`

| Attribute            | Type     | Description                                                           |
|----------------------|----------|-----------------------------------------------------------------------|
| `icon-fallback-text` | `string` | *Required* The fallback text for the icon. Cannot be `null` or empty. |

The content is the HTML to use within the generated component.
