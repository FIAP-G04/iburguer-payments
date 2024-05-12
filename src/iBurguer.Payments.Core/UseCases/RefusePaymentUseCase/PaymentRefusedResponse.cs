using iBurguer.Payments.Core.Domain;

namespace iBurguer.Payments.Core.UseCases.RefusePaymentUseCase;

public record PaymentRefusedResponse
{
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime RefusedAt { get; set; }

    public static PaymentRefusedResponse Convert(Payment payment)
    {
        return new PaymentRefusedResponse
        {
            PaymentId = payment.Id,
            OrderId = payment.OrderId,
            PaymentStatus = payment.Status.Name,
            RefusedAt = payment.RefusedAt!.Value
        };
    }
}