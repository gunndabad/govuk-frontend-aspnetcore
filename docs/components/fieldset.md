# Fieldset

[GDS Fieldset component](https://design-system.service.gov.uk/components/fieldset/)

## Example

```razor
<govuk-fieldset>
    <govuk-fieldset-legend is-page-heading="true" class="govuk-fieldset__legend--l">Legend as page heading</govuk-fieldset-legend>
</govuk-fieldset>
```

![Fieldset](../images/fieldset.png)


## API

### `<govuk-fieldset>`

| Attribute      | Type     | Description                                                         |
|----------------|----------|---------------------------------------------------------------------|
| `described-by` | `string` | One or more element IDs to add to the `aria-describedby` attribute. |
| `role`         | `string` | The `role` attribute for the fieldset.                              |

### `<govuk-fieldset-legend>`

*Required*\
The content is the HTML to use within the legend.\
Must be inside a `<govuk-fieldset>` element.

| Attribute         | Type      | Description |
|-------------------|-----------| --- |
| `is-page-heading` | `boolean` | Whether the legend also acts as the heading for the page. The default is `false`. |
