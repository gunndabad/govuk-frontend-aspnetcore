# File upload

[GDS File upload component](https://design-system.service.gov.uk/components/file-upload/)

## Example

```razor
<govuk-file-upload name="FileUpload1">
    <govuk-file-upload-label>Upload a file</govuk-file-upload-label>
</govuk-file-upload>
```

![File upload](../images/file-upload.png)

## Example - with error message

```razor
<govuk-file-upload name="FileUpload1">
    <govuk-file-upload-label>Upload a file</govuk-file-upload-label>
    <govuk-file-upload-error-message>The CSV must be smaller than 2MB</govuk-file-upload-error-message>
</govuk-file-upload>
```

![File upload](../images/file-upload-with-errors.png)


## API

### `<govuk-file-upload>`

| Attribute | Type | Description |
| --- | --- | --- |
| `asp-for` | `ModelExpression` | The model expression used to generate the `name` and `id` attributes as well as the error message content. See [documentation on forms](forms.md) for more information. |
| `described-by` | `string` | One or more element IDs to add to the `aria-describedby` attribute of the generated `input` element. |
| `id` | `string` | The `id` attribute for the generated `input` element. If not specified then a value is generated from the `name` attribute. |
| `ignore-modelstate-errors` | `bool` | Whether ModelState errors on the ModelExpression specified by the `asp-for` attribute should be ignored when generating an error message. The default is `false`. |
| `input-*` | | Additional attributes to add to the generated `input` element. |
| `label-class` | `string` | Additional classes for the generated `label` element. |
| `name` | `string` | The `name` attribute for the generated `input` element. Required unless the `asp-for` attribute is specified. |

### `<govuk-file-upload-label>`

The content is the HTML to use within the component's label.\
Must be inside a `<govuk-file-upload>` element.

| Attribute | Type | Description |
| --- | --- | --- |
| `is-page-heading` | `bool` | Whether the label also acts as the heading for the page. The default is `false`. |

### `<govuk-file-upload-hint>`

The content is the HTML to use within the component's hint.\
Must be inside a `<govuk-file-upload>` element.

If the `asp-for` attribute is specified on the parent `<govuk-file-upload>` then content for the hint will be generated from the model expression.\
If you want to retain the generated content and specify additional attributes then use a self-closing tag e.g.
`<govuk-file-upload-hint class="some-additional-class" />`.

### `<govuk-file-upload-error-message>`

The content is the HTML to use within the component's error message.\
Must be inside a `<govuk-file-upload>` element.

If the `asp-for` attribute is specified on the parent `<govuk-file-upload>` then content for the error message will be generated from the model expression.
(To prevent this set `ignore-modelstate-errors` on the parent `<govuk-file-upload>` to `false`.) Specifying any content here will override any generated error message.\
If you want to retain the generated content and specify additional attributes then use a self-closing tag e.g.
`<govuk-file-upload-error-message visually-hidden-text="Error" />`.

| Attribute | Type | Description |
| --- | --- | --- |
| `visually-hidden-text` | `string` | The visually hidden prefix used before the error message. The default is `Error`. |
