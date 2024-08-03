using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.UseCases.GenerateQrCode;
using static iBurguer.Payments.Core.Exceptions;

namespace iBurguer.Payments.Core.UseCases.GetPaymentUseCase
{
    public interface IGetPaymentUseCase
    {
        Task<GenerateQrCodeResponse> GetPayment(Guid orderId, CancellationToken cancellationToken);
    }

    public class GetPaymentUseCase : IGetPaymentUseCase
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentUseCase(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<GenerateQrCodeResponse> GetPayment(Guid orderId, CancellationToken cancellationToken)
        {
            var payment = await _repository.GetByOrderId(orderId, cancellationToken);

            PaymentNotFoundException.ThrowIfNull(payment);

            return GenerateQrCodeResponse.Convert(payment);
        }
    }
}
