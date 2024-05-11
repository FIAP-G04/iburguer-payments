namespace iBurguer.Payments.Core.Gateways;

public interface IPaymentGateway
{
    Task<string> GenerateQrCode(Guid orderId, CancellationToken cancellationToken);
}