using AutoFixture.Xunit2;
using FluentAssertions;
using iBurguer.Payments.Core.Gateways;
using iBurguer.Payments.Infrastructure.Http.MercadoPago;

namespace iBurguer.Payments.UnitTests.Infrastructure.Http;

public class MercadoPagoClientTests
{
    private readonly IPaymentGateway _sut = new MercadoPagoClient();

    [Theory, AutoData]
    public async Task ShouldGenerateQrCodeOnMercadoPago(Guid orderId)
    {
        // Act
        var result = await _sut.GenerateQrCode(orderId, CancellationToken.None);
        
        // Assert
        result.Should()
            .Be(
                "00020101021243650016COM.MERCADOLIBRE02013063638f1192a-5fd1-4180-a180-8bcae3556bc35204000053039865802BR5925IZABELAAAADEMELO6007BARUERI6207050363040B6D");
    }
}