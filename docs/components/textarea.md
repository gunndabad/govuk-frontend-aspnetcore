# Textarea

[GDS Textarea component](https://design-system.service.gov.uk/components/textarea/)

## Example

```razor
<govuk-textarea name="more-detail">
    <govuk-textarea-label class="govuk-label--l" is-page-heading="true">
        Can you provide more detail?
    </govuk-textarea-label>
    <govuk-textarea-hint>
        Do not include personal or financial information like your National Insurance number or credit card details.
    </govuk-textarea-hint>
</govuk-textarea>
```

![Textarea](../images/textarea.png)


## API

### `<govuk-textarea>`

| Attribute | Type | Description |
| --- | --- | --- |
| `asp-for` | `ModelExpression` | The model expression used to generate the `name` and `id` attributes, the content as well as the error message content. See [documentation on forms](forms.md) for more information. |
| `autocomplete`| `string` | The `autocomplete` attribute for the generated `textarea` element. |
| `disabled` | `bool` | Whether the textarea should be disabled. The default is `false`. |
| `id` | `string` | The `id` attribute for the generated `textarea` element. If not specified then a value is generated from the `name` attribute. |
| `ignore-modelstate-errors` | `bool` | Whether ModelState errors on the ModelExpression specified by the `asp-for` attribute should be ignored when generating an error message. The default is `false`. |
| `label-class` | `string` | Additional classes for the generated `label` element. |
| `name` | `string` | The `name` attribute for the generated `textarea` element. Required unless the `asp-for` attribute is specified. |
| `rows` | `int` | The `rows` attribute for the generated `textarea` element. The default is `5`. |
| `spellcheck` | `bool?` | The `spellcheck` attribute for the generated `textarea` element. The default is `null`. |
| `textarea-*` | | Additional attributes to add to the generated `textarea` element. |

### `<govuk-textarea-label>`

The content is the HTML to use within the component's label.\
Must be inside a `<govuk-textarea>` element.

| Attribute | Type | Description |
| --- | --- | --- |
| `is-page-heading` | `bool` | Whether the label also acts as the heading for the page. The default is `false`. |

### `<govuk-textarea-hint>`

The content is the HTML to use within the component's hint.\
Must be inside a `<govuk-textarea>` element.

If the `asp-for` attribute is specified on the parent `<govuk-textarea>` then content for the hint will be generated from the model expression.\
If you want to retain the generated content and specify additional attributes then use a self-closing tag e.g.
`<govuk-textarea-hint class="some-additional-class" />`.

### `<govuk-textarea-error-message>`

The content is the HTML to use within the component's error message.\
Must be inside a `<govuk-textarea>` element.

If the `asp-for` attribute is specified on the parent `<govuk-textarea>` then content for the error message will be generated from the model expression.
(To prevent this set `ignore-modelstate-errors` on the parent `<govuk-textarea>` to `false`.) Specifying any content here will override any generated error message.\
If you want to retain the generated content and specify additional attributes then use a self-closing tag e.g.
`<govuk-textarea-error-message visually-hidden-text="Error" />`.

| Attribute | Type | Description |
| --- | --- | --- |
| `visually-hidden-text` | `string` | The visually hidden prefix used before the error message. The default is `Error`. |

### `<govuk-textarea-value>`

The content is the HTML to use within the generated `textarea`.\
Must be inside a `<govuk-textarea>` element.
