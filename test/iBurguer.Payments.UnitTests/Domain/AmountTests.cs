using FluentAssertions;
using static iBurguer.Payments.Core.Exceptions;
using iBurguer.Payments.Core.Domain;

namespace iBurguer.Payments.UnitTests.Domain;

public class AmountTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(1000)]
    public void ShouldReturnValidValue(decimal value)
    {
        // Arrange & Act
        var amount = new Amount(value);

        // Assert
        amount.Value.Should().Be(value);
    }

    [Fact]
    public void ShouldThrowExceptionForNegativeValues()
    {
        // Arrange, Act & Assert
        Assert.Throws<InvalidAmountException>(() => new Amount(-1));
    }

    [Fact]
    public void ShouldReturnCorrectTextualRepresentation()
    {
        // Arrange
        var amount = new Amount(50);

        // Act
        var result = amount.ToString();

        // Assert
        result.Should().Be("50");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(1000)]
    public void ShouldPerformImplicitConversionFromDecimalToAmountCorrectly(decimal value)
    {
        // Arrange
        Amount amount = new Amount(value);

        // Act
        decimal result = amount;

        // Assert
        result.Should().Be(value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(1000)]
    public void ShouldPerformImplicitConversionFromAmountToDecimalCorrectly(decimal value)
    {
        // Arrange
        Amount amount = value;

        // Act
        decimal result = amount.Value;

        // Assert
        result.Should().Be(value);
    }
}