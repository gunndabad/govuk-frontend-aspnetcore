# Max words validator

To perform server-side validation that matches the JavaScript validation used in the [Character count component](../components/character-count.md) you can use the `GovUk.Frontend.AspNetCore.Validation.MaxWordsAttribute`.

### Example

```cs
using GovUk.Frontend.AspNetCore.Validation;

public class MyModel
{
    [MaxWords(words: 150, ErrorMessage = "Job description must be 150 words or fewer")]
    public string JobDescription { get; set; }
}
```

## Usage with alternative validation frameworks

If you're not using data annotation attributes for validation, a helper class is provided for integrating with other validation frameworks - `GovUk.Frontend.AspNetCore.Validation.MaxWordsValidator`.

### Example with Fluent Validation

#### Custom validator
```cs
using FluentValidation;
using GovUk.Frontend.AspNetCore.Validation;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> MaxWords<T>(this IRuleBuilder<T, string> ruleBuilder, int maxWords) =>
        ruleBuilder.Must(value =>
        {
            var validator = new MaxWordsValidator(maxWords);
            return validator.IsValid(value);
        });
}
```

#### Usage
```cs
public class MyModel
{
    public string JobDescription { get; set; }
}

public class MyModelValidator : AbstractValidator<MyModel>
{
    public MyModelValidator()
    {
        RuleFor(m => m.JobDescription).MaxWords(150).WithMessage("Job description must be 150 words or fewer");
    }
}
```
