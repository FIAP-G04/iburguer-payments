using iBurguer.Payments.Core.Abstractions;

namespace iBurguer.Payments.Core;

public static class Exceptions
{
    public class CannotToConfirmPayment() : DomainException<CannotToConfirmPayment>("Only payments in the 'Pending' state can confirmed.");

    public class CannotToRefusePayment() : DomainException<CannotToRefusePayment>("Only payments in the 'Pending' state can be refused.");
    
    public class InvalidAmount() : DomainException<InvalidAmount>("The amount cannot have a value equal to zero or negative");

    public class PaymentNotFound() : DomainException<PaymentNotFound>("No payment was found with the specified ID");
    
    public class ErrorInPaymentProcessing() : DomainException<ErrorInPaymentProcessing>("An error occurred while processing the payment. Try again later.");
}