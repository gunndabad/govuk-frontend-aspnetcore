# Error message

[GDS Error message component](https://design-system.service.gov.uk/components/error-message/)

## Example - specified content

```razor
<govuk-error-message>Enter your full name</govuk-error-message>
```

![Error message](../images/error-message-with-specified-content.png)

## Example - overriden visually hidden text

```razor
<govuk-error-message visually-hidden-text="Gwall">Rhowch eich enw llawn</govuk-error-message>
```

![Error message](../images/error-message-with-overriden-visually-hidden-text.png)

## Example - ModelState error

```razor
<govuk-error-message for="FullName" />
```

![Error message](../images/error-message-with-modelstate-error.png)

## API

### `<govuk-error-message>`

The content is the HTML to use within the generated error message. Content is required if the `for` attribute is not specified.

If `for` is specified and there are no errors in `ModelState` then no output will be generated. If there are multiple errors only the first will be used.

| Attribute              | Type              | Description                                                                                                                                                                 |
|------------------------|-------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `for`                  | `ModelExpression` | The model expression used to generate the error message. If content is specified this attribute is ignored. See [documentation on forms](../forms.md) for more information. |
| `visually-hidden-text` | `string`          | The visually hidden prefix used before the error message. The default is `Error`.                                                                                           |
