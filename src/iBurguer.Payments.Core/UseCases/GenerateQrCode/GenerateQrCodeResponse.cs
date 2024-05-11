using iBurguer.Payments.Core.Domain;

namespace iBurguer.Payments.Core.UseCases.GenerateQrCode;

public record GenerateQrCodeResponse
{
    public required Guid PaymentId { get; set; }
    public required Guid OrderId { get; set; }
    public required string PaymentStatus { get; set; }
    public required string QrData { get; set; }
    public required DateTime CreatedAt { get; set; }

    public static GenerateQrCodeResponse Convert(Payment payment)
    {
        return new GenerateQrCodeResponse
        {
            PaymentId = payment.Id,
            OrderId = payment.OrderId,
            CreatedAt = payment.CreatedAt,
            PaymentStatus = payment.Status.Name,
            QrData = payment.QrData
        };
    }
}