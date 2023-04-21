# Tabs

[GDS Tabs component](https://design-system.service.gov.uk/components/tabs/)

## Example

```razor
<govuk-tabs>
    <govuk-tabs-item id="past-day" label="Past day">
        <h2 class="govuk-heading-l">Past day</h2>
    </govuk-tabs-item>
    <govuk-tabs-item id="past-week" label="Past week">
        <h2 class="govuk-heading-l">Past week</h2>
    </govuk-tabs-item>
    <govuk-tabs-item id="past-month" label="Past month">
        <h2 class="govuk-heading-l">Past month</h2>
    </govuk-tabs-item>
    <govuk-tabs-item id="past-year" label="Past year">
        <h2 class="govuk-heading-l">Past year</h2>
    </govuk-tabs-item>
</govuk-tabs>
```

![Tag](../images/tabs.png)


## API

### `<govuk-tabs>`

| Attribute | Type | Description |
| --- | --- | --- |
| `id` | `string` | The `id` attribute for the main tabs component. |
| `id-prefix` | `string` | The prefix to use when generating IDs for the items. Required unless every item specifies the `id` attribute. |
| `title` | `string` | The title for the tabs table of contents. The default is `Contents`. |

### `<govuk-tabs-item>`

The content is the HTML of the panel.\
Must be inside a `<govuk-tabs>` element.

| Attribute | Type | Description |
| --- | --- | --- |
| `id` | `string` | The `id` attribute for the tab. Required unless the `id-prefix` attribute is specified on the parent `<govuk-tabs>`.  |
| `label` | `string` | *Required* The text label of the tab. |
| (link attributes) | | Additional attributes to add to the generated link to this tab.  |
