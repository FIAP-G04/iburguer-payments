using iBurguer.Payments.Core.Domain;
using static iBurguer.Payments.Core.Exceptions;

namespace iBurguer.Payments.Core.UseCases.RefusePaymentUseCase;

public interface IRefusePaymentUseCase
{
    Task<PaymentRefusedResponse> RefusePayment(Guid paymentId, CancellationToken cancellationToken);
}

public class RefusePaymentUseCase : IRefusePaymentUseCase
{
    private readonly IPaymentRepository _repository;

    public RefusePaymentUseCase(IPaymentRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public async Task<PaymentRefusedResponse> RefusePayment(Guid paymentId, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetById(paymentId, cancellationToken);

        PaymentNotFound.ThrowIfNull(payment);

        payment.Refuse();

        var refused = await _repository.Update(payment, cancellationToken);
        
        ErrorInPaymentProcessing.ThrowIf(!refused);

        return PaymentRefusedResponse.Convert(payment);
    }
}