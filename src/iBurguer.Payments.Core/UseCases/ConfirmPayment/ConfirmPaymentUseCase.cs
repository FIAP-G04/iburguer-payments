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

    public ConfirmPaymentUseCase(IPaymentRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<PaymentConfirmedResponse> ConfirmPayment(Guid paymentId, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetById(paymentId, cancellationToken);

        PaymentNotFoundException.ThrowIfNull(payment);

        payment!.Confirm();

        var processed = await _repository.Update(payment, cancellationToken);
        
        ErrorInPaymentProcessingException.ThrowIf(!processed);

        return PaymentConfirmedResponse.Convert(payment);
    }
}