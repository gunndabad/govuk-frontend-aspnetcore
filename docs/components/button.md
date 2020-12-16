# Button

[GDS Button component](https://design-system.service.gov.uk/components/button/)

There are two tag helpers for the button component. `<govuk-button>` generates a `<button>` element; `<govuk-button-link>` generates an 'a' element.

## Example - default button

```razor
<govuk-button type="submit">Save and continue</govuk-button>
```

![Button](../images/button-default.png)

## Example - secondary button

```razor
<govuk-button class="govuk-button--secondary">Cancel</govuk-button>
```

![Button](../images/button-secondary.png)

## Example - start button

```razor
<govuk-button-link is-start-button="true" href="/start">Start</govuk-button>
```

![Button](../images/button-start.png)

## Example - disabled

```razor
<govuk-button disabled="true">Save and continue</govuk-button>
```

![Button](../images/button-disabled.png)

## Example - link

```razor
<govuk-button-link href="/">Confirm</govuk-button>
```

## Example - generated link

```razor
<govuk-button-link asp-controller="Home" asp-action="Confirm">Confirm</govuk-button>
```

## Example - generated formaction

```razor
<govuk-button type="submit" asp-controller="Home" asp-action="Confirm">Confirm</govuk-button>
```

## API

### `<govuk-button>`

Content is the inner HTML for the button.

| Attribute | Type | Description |
| --- | --- | --- |
| `disabled` | `bool` | Whether the button should be disabled. `disabled` and `aria-disabled` attributes will be set automatically. Default is `false`. |
| `is-start-button` | `bool` | Use for the main call to action on your service's start page. Default is `false`. |
| `prevent-double-click` | `bool` | Prevent accidental double clicks on submit buttons from submitting forms multiple times. Default is `false`. |
| (link attributes) | | If specified generates a `formaction` attribute using the specified values. See [documentation on links](../links.md) for more information. |
| * | | Any additional attributes will be copied onto the generated `<button>`. |

### `<govuk-button-link>`

Content is the inner HTML for the button.

| Attribute | Type | Description |
| --- | --- | --- |
| `disabled` | `bool` | Whether the button should be disabled. `disabled` and `aria-disabled` attributes will be set automatically. Default is `false`. |
| `is-start-button` | `bool` | Use for the main call to action on your service's start page. Default is `false`. |
| (link attributes) | | If specified generates an `href` attribute using the specified values. See [documentation on links](../links.md) for more information. |
| * | | Any additional attributes will be copied onto the generated `<a>`. |
