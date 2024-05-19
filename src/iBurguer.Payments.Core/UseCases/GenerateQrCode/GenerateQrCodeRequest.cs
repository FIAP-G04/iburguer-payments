using FluentValidation;

namespace iBurguer.Payments.Core.UseCases.GenerateQrCode;

public record GenerateQrCodeRequest
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}