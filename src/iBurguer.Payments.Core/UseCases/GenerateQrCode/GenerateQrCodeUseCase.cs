using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.Gateways;

namespace iBurguer.Payments.Core.UseCases.GenerateQrCode;

public interface IGenerateQrCodeUseCase
{
    Task<GenerateQrCodeResponse> GenerateQrCode(GenerateQrCodeRequest request, CancellationToken cancellationToken);
}

public class GenerateQrCodeUseCase : IGenerateQrCodeUseCase
{
    private readonly IPaymentRepository _repository;
    private readonly IPaymentGateway _gateway;

    public GenerateQrCodeUseCase(IPaymentRepository repository, IPaymentGateway gateway)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(gateway);

        _repository = repository;
        _gateway = gateway;
    }

    public async Task<GenerateQrCodeResponse> GenerateQrCode(GenerateQrCodeRequest request, CancellationToken cancellationToken)
    {
        var qrData = await _gateway.GenerateQrCode(request.OrderId, cancellationToken);
        
        var payment = new Payment(request.OrderId, request.Amount, qrData);

        await _repository.Save(payment);

        return GenerateQrCodeResponse.Convert(payment);
    }
}