using iBurguer.Payments.Core.Domain;

namespace iBurguer.Payments.Core.UseCases.ConfirmPayment;

public record PaymentConfirmedResponse
{
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime PayedAt { get; set; }

    public static PaymentConfirmedResponse Convert(Payment payment)
    {
        return new PaymentConfirmedResponse
        {
            PaymentId = payment.Id,
            OrderId = payment.OrderId,
            PaymentStatus = payment.Status.Name,
            PayedAt = payment.PayedAt!.Value
        };
    }
}