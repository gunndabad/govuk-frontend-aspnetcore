using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GovUk.Frontend.AspNetCore.Validation;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.Validation
{
    public class MaxWordsAttributeTests
    {
        [Fact]
        public void ValidInput_PassesValidation()
        {
            // Arrange
            var model = new Model() { TheString = "first second" };
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void InvalidInput_PassesValidation()
        {
            // Arrange
            var model = new Model() { TheString = "first second third forth" };
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);

            Assert.Collection(
                validationResults,
                result => Assert.Equal("TheString must be 3 words or fewer", result.ErrorMessage));
        }

        public class Model
        {
            [MaxWords(words: 3, ErrorMessage = "TheString must be 3 words or fewer")]
            public string TheString { get; set; }
        }
    }
}
