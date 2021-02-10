# Details

[GDS Details component](https://design-system.service.gov.uk/components/details/)

## Example

```razor
<govuk-details>
    <govuk-details-summary>
        Help with nationality
    </govuk-details-summary>
    <govuk-details-text>
        We need to know your nationality so we can work out which elections you’re entitled to vote in.
        If you cannot provide your nationality, you’ll have to send copies of identity documents through the post.
    </govuk-details-text>
</govuk-details>
```

![Details](../images/details.png)

## Example - expanded

```razor
<govuk-details open="true">
    <govuk-details-summary>
        Help with nationality
    </govuk-details-summary>
    <govuk-details-text>
        We need to know your nationality so we can work out which elections you’re entitled to vote in.
        If you cannot provide your nationality, you’ll have to send copies of identity documents through the post.
    </govuk-details-text>
</govuk-details>
```

![Details](../images/details-expanded.png)

## API

### `<govuk-details>`

| Attribute | Type | Description |
| --- | --- | --- |
| `open` | `bool` | Whether the details element should be expanded. The default is `false`. |

### `<govuk-details-summary>`

*Required*\
The content is the HTML to use within the generated summary element.\
Must be inside a `<govuk-details>` element.

### `<govuk-details-text>`

*Required*\
The content is the HTML to use within the disclosed part of the details element.\
Must be inside a `<govuk-details>` element.
