namespace iBurguer.Payments.Core.Gateways;

public interface IOrderGateway
{
    Task<bool> ConfirmOrder(Guid orderId, CancellationToken cancellationToken = default);
    
    Task<bool> CancelOrder(Guid orderId, CancellationToken cancellationToken = default);
}