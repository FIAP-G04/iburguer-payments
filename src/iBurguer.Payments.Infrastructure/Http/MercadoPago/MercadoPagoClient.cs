using iBurguer.Payments.Core.Gateways;

namespace iBurguer.Payments.Infrastructure.Http.MercadoPago;

public class MercadoPagoClient : IPaymentGateway
{
    public async Task<string> GenerateQrCode(Guid orderId, CancellationToken cancellationToken)
    {
        return
            "00020101021243650016COM.MERCADOLIBRE02013063638f1192a-5fd1-4180-a180-8bcae3556bc35204000053039865802BR5925IZABELAAAADEMELO6007BARUERI6207050363040B6D";
    }
}