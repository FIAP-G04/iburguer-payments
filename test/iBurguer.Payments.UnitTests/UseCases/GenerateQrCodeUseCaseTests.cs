using System.Runtime.InteropServices.JavaScript;
using AutoFixture.Xunit2;
using FluentAssertions;
using static iBurguer.Payments.Core.Exceptions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.Gateways;
using iBurguer.Payments.Core.UseCases.GenerateQrCode;
using iBurguer.Payments.UnitTests.Util;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace iBurguer.Payments.UnitTests.UseCases;

public class GenerateQrCodeUseCaseTests : BaseTests
{
    private readonly IPaymentRepository _repository;
    private readonly IPaymentGateway _gateway;
    private readonly IGenerateQrCodeUseCase _sut;

    public GenerateQrCodeUseCaseTests()
    {
        _repository = Substitute.For<IPaymentRepository>();
        _gateway = Substitute.For<IPaymentGateway>();
        _sut = new GenerateQrCodeUseCase(_repository, _gateway);
    }

    [Theory, AutoData]
    public async Task ShouldGenerateQrCodeAndSavePayment(GenerateQrCodeRequest request, string qrCode)
    {
        // Arrange
        _gateway.GenerateQrCode(request.OrderId, Arg.Any<CancellationToken>()).Returns(qrCode);

        // Act
        var result = await _sut.GenerateQrCode(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PaymentId.Should().NotBeEmpty();
        result.OrderId.Should().Be(request.OrderId);
        result.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        result.PaymentStatus.Should().Be(PaymentStatus.Pending.Name);
        result.QrData.Should().Be(qrCode);

        await _repository.Received(1).Save(Arg.Is<Payment>(p => p.Id == result.PaymentId));
    }
}