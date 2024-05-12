using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.Gateways;
using static iBurguer.Payments.Core.Exceptions;

namespace iBurguer.Payments.Core.UseCases.ConfirmPayment;

public interface IConfirmPaymentUseCase
{
    Task<PaymentConfirmedResponse> ConfirmPayment(Guid paymentId, CancellationToken cancellationToken);
}

public class ConfirmPaymentUseCase : IConfirmPaymentUseCase
{
    private readonly IPaymentRepository _repository;
    private readonly IOrderGateway _gateway;

    public ConfirmPaymentUseCase(IPaymentRepository repository, IOrderGateway gateway)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(gateway);

        _repository = repository;
        _gateway = gateway;
    }

    public async Task<PaymentConfirmedResponse> ConfirmPayment(Guid paymentId, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetById(paymentId, cancellationToken);

        PaymentNotFound.ThrowIfNull(payment);

        payment!.Confirm();

        var processed = await _repository.Update(payment, cancellationToken);
        
        ErrorInPaymentProcessing.ThrowIf(!processed);

        return PaymentConfirmedResponse.Convert(payment);
    }
}