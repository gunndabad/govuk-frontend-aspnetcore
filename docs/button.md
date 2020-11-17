# Button

[GDS Button component](https://design-system.service.gov.uk/components/button/)

## Example - default button

```razor
<govuk-button type="submit">Save and continue</govuk-button>
```

## Example - secondary button

```razor
<govuk-button class="govuk-button--secondary">Cancel</govuk-button>
```

## Example - start button

```razor
<govuk-button is-start-button="true" href="/start">Start</govuk-button>
```

## Example - disabled

```razor
<govuk-button disabled="true">Save and continue</govuk-button>
```

## Example - link

```razor
<govuk-button href="/">Confirm</govuk-button>
```

## Example - generated link

```razor
<govuk-button asp-controller="Home" asp-action="Confirm">Confirm</govuk-button>
```

## Example - generated formaction

```razor
<govuk-button type="submit" asp-controller="Home" asp-action="Confirm">Confirm</govuk-button>
```

## API

### `<govuk-button>`

Content is the HTML for the button or link.

| Attribute | Type | Description |
| --- | --- | --- |
| `asp-*` | | TODO |
| `formaction` | `string` | The URL that processes the information submitted by the button. Overrides the `action` attribute of the button's form owner. Specifying this attribute for `a` elements is not permitted. |
| `disabled` | `bool?` | Whether the button should be disabled. For `button` elements, `disabled` and `aria-disabled` attributes will be set automatically. Default is `false`. |
| `href` | `string` | The URL that the button should link to. If this attribute is specified an `a` element is rendered. |
| `is-start-button` | `bool?` | Use for the main call to action on your service's start page. Default is `false`. |
| `name` | `string` | Name for the `button`. Specifying this attribute for `a` elements is not permitted. |
| `prevent-double-click` | `bool?` | Prevent accidental double clicks on submit buttons from submitting forms multiple times. Specifying this attribute for `a` elements is not permitted. Default is `false`. |
| `type` | `string` | Type of `button` - `button`, `submit` or `reset`. If this attribute is specified a `button` element is rendered. Default is `submit`. |
| `value` | `string` | Value for the `button` tag. Specifying this attribute for `a` elements is not permitted. |