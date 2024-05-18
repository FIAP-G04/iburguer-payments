using static iBurguer.Payments.Core.Exceptions;
using iBurguer.Payments.Core.Domain;

namespace iBurguer.Payments.UnitTests.Domain;

public class AmountTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(1000)]
    public void ShouldReturnValidValue(decimal amount)
    {
        // Arrange & Act
        var value = new Amount(amount);

        // Assert
        Assert.Equal(amount, value.Value);
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
        Assert.Equal("50", result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(1000)]
    public void ShouldPerformImplicitConversionFromDecimalToAmountCorrectly(decimal amount)
    {
        // Arrange
        Amount amountObj = new Amount(amount);

        // Act
        decimal result = amountObj;

        // Assert
        Assert.Equal(amount, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(1000)]
    public void ShouldPerformImplicitConversionFromAmountToDecimalCorrectly(decimal amount)
    {
        // Arrange
        Amount amountObj = amount;

        // Act
        decimal result = amountObj.Value;

        // Assert
        Assert.Equal(amount, result);
    }
}