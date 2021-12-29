using System;
using GovUk.Frontend.AspNetCore.Validation;
using Xunit;

namespace GovUk.Frontend.AspNetCore.Tests.Validation
{
    public class MaxWordsValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Ctor_InvalidMaxWords_ThrowsArgumentNullException(int maxWords)
        {
            // Arrange

            // Act
            var ex = Record.Exception(() => new MaxWordsValidator(maxWords));

            // Assert
            var argException = Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal("maxWords", argException.ParamName);
        }

        [Fact]
        public void IsValid_NullInput_ThrowsArgumentNullException()
        {
            // Arrange
            var validator = new MaxWordsValidator(maxWords: 3);

            // Act
            var ex = Record.Exception(() => validator.IsValid(value: null));

            // Assert
            var argException = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal("value", argException.ParamName);
        }

        [Fact]
        public void IsValid_EmptyInput_ReturnsTrue()
        {
            // Arrange
            var validator = new MaxWordsValidator(maxWords: 3);

            // Act
            var result = validator.IsValid(value: "");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_InputLessThanMaxWords_ReturnsTrue()
        {
            // Arrange
            var validator = new MaxWordsValidator(maxWords: 3);
            var input = "first second";

            // Act
            var result = validator.IsValid(value: input);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("first second third")]
        [InlineData("first  second third")]
        [InlineData("first\nsecond\nthird")]
        [InlineData("first\nsecond\n  third")]
        public void IsValid_InputEqualMaxWords_ReturnsTrue(string input)
        {
            // Arrange
            var validator = new MaxWordsValidator(maxWords: 3);

            // Act
            var result = validator.IsValid(value: input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_InputMoreThanMaxWords_ReturnsFalse()
        {
            // Arrange
            var validator = new MaxWordsValidator(maxWords: 3);
            var input = "first second third forth";

            // Act
            var result = validator.IsValid(value: input);

            // Assert
            Assert.False(result);
        }
    }
}
