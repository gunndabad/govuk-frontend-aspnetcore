# Accordion

[GDS Accordion component](https://design-system.service.gov.uk/components/accordion/)

## Example

```razor
<govuk-accordion id="accordion-with-summary-sections" heading-level="3">
    <govuk-accordion-item expanded="true">
        <govuk-accordion-item-heading>Understanding agile project management</govuk-accordion-item-heading>
        <govuk-accordion-item-summary>Introductions, methods, core features.</govuk-accordion-item-summary>
        <ul class="govuk-list">
            <li>
                <a class="govuk-link" href="#">Agile and government services: an introduction</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Agile methods: an introduction</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Core principles of agile</a>
            </li>
        </ul>
    </govuk-accordion-item>
    <govuk-accordion-item>
        <govuk-accordion-item-heading>Working with agile methods</govuk-accordion-item-heading>
        <govuk-accordion-item-summary>Workspaces, tools and techniques, user stories, planning.</govuk-accordion-item-summary>
        <ul class="govuk-list">
            <li>
                <a class="govuk-link" href="#">Creating an agile working environment</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Agile tools and techniques</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Set up a team wall</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Writing user stories</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Planning in agile</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Deciding on priorities</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Developing a roadmap</a>
            </li>
        </ul>
    </govuk-accordion-item>
    <govuk-accordion-item>
        <govuk-accordion-item-heading>Governing agile services</govuk-accordion-item-heading>
        <govuk-accordion-item-summary>Principles, measuring progress, spending money.</govuk-accordion-item-summary>
        <ul class="govuk-list">
            <li>
                <a class="govuk-link" href="#">Governance principles for agile service delivery</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Measuring and reporting progress</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Spend controls: check if you need approval to spend money on a service</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Spend controls: apply for approval to spend money on a service</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Spend controls: the new pipeline process</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Working across organisational boundaries</a>
            </li>
        </ul>
    </govuk-accordion-item>
    <govuk-accordion-item>
        <govuk-accordion-item-heading>Phases of an agile project</govuk-accordion-item-heading>
        <govuk-accordion-item-summary>Discovery, alpha, beta, live and retirement.</govuk-accordion-item-summary>
        <ul class="govuk-list">
            <li>
                <a class="govuk-link" href="#">How the discovery phase works</a>
            </li>
            <li>
                <a class="govuk-link" href="#">How the alpha phase works</a>
            </li>
            <li>
                <a class="govuk-link" href="#">How the beta phase works</a>
            </li>
            <li>
                <a class="govuk-link" href="#">How the live phase works</a>
            </li>
            <li>
                <a class="govuk-link" href="#">Retiring your service</a>
            </li>
        </ul>
    </govuk-accordion-item>
</govuk-accordion>
```

![Accordion](images/accordion-with-summary-sections.png)

## API

### `<govuk-accordion>`

| Attribute | Type | Description |
| --- | --- | --- |
| `id` | `string` | *Required* Must be **unique** across the domain of your service (as the expanded state of individual instances of the component persists across page loads using [`sessionStorage`](https://developer.mozilla.org/en-US/docs/Web/API/Window/sessionStorage)). Used as an `id` in the HTML for the accordion as a whole, and also as a prefix for the `id`s of the section contents and the buttons that open them, so that those `id`s can be the target of `aria-labelledby` and `aria-control` attributes. |
| `heading-level` | `int` | Heading level, from 1 to 6. Default is `2`. |

### `<govuk-accordion-item>`

Content is the HTML of each section, which is hidden when the section is closed.\
Must be inside a `<govuk-accordion>` element.

| Attribute | Type | Description |
| --- | --- | --- |
| `expanded` | `bool` | Whether the section should be expanded upon initial load or not. Default is `false`. |

### `<govuk-accordion-item-heading>`

Content is the HTML of the header for each section which is used both as the title for each section, and as the button to open or close each section.\
Must be inside a `<govuk-accordion-item>` element.

### `<govuk-accordion-item-summary>`

Content is the HTML for summary line.\
Must be inside a `<govuk-accordion-item>` element.
