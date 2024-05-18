using AutoFixture.Xunit2;
using FluentAssertions;
using iBurguer.Payments.API.Controllers;
using iBurguer.Payments.Core.UseCases.ConfirmPayment;
using iBurguer.Payments.Core.UseCases.GenerateQrCode;
using iBurguer.Payments.Core.UseCases.RefusePaymentUseCase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace iBurguer.Payments.UnitTests.API;

public class PaymentControllerTests
{
    public readonly PaymentController _sut;

    public PaymentControllerTests()
    {
        _sut = new PaymentController();
    }
    
    [Theory, AutoData]
    public async Task ShouldGenerateQrCode(GenerateQrCodeRequest request, GenerateQrCodeResponse response)
    {
        // Arrange
        var useCase = Substitute.For<IGenerateQrCodeUseCase>();
        useCase.GenerateQrCode(request, CancellationToken.None).Returns(response);

        // Act
        var result = await _sut.GenerateQrCode(useCase, request, CancellationToken.None) as CreatedResult;

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.Value.Should().BeOfType<GenerateQrCodeResponse>();
        result.Value.Should().BeEquivalentTo(response);
    }
    
    [Theory, AutoData]
    public async Task ShouldConfirmPayment(Guid paymentId, PaymentConfirmedResponse response)
    {
        // Arrange
        var useCase = Substitute.For<IConfirmPaymentUseCase>();
        useCase.ConfirmPayment(paymentId, CancellationToken.None).Returns(response);

        // Act
        var result = await _sut.ConfirmPayment(useCase, paymentId, CancellationToken.None) as OkObjectResult;

        // Assert
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeOfType<PaymentConfirmedResponse>();
        result.Value.Should().BeEquivalentTo(response);
    }
    
    [Theory, AutoData]
    public async Task ShouldRefusePayment(Guid paymentId, PaymentRefusedResponse response)
    {
        // Arrange
        var useCase = Substitute.For<IRefusePaymentUseCase>();
        useCase.RefusePayment(paymentId, CancellationToken.None).Returns(response);

        // Act
        var result = await _sut.RefusePayment(useCase, paymentId, CancellationToken.None) as OkObjectResult;

        // Assert
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeOfType<PaymentRefusedResponse>();
        result.Value.Should().BeEquivalentTo(response);
    }
}

