using FluentValidation;

namespace iBurguer.Payments.Core.UseCases.GenerateQrCode;

public class GenerateQrCodeRequest
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    
    public class Validator : AbstractValidator<GenerateQrCodeRequest>
    {
        public Validator()
        {
            RuleFor(r => r.OrderId).NotEmpty();
            RuleFor(r => r.Amount).NotEmpty().GreaterThan((ushort)0);
        }
    }
}