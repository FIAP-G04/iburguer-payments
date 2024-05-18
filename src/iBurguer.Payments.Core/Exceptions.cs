using iBurguer.Payments.Core.Abstractions;

namespace iBurguer.Payments.Core;

public static class Exceptions
{
    public class CannotToConfirmPaymentException() : DomainException<CannotToConfirmPaymentException>("Only payments in the 'Pending' state can confirmed.");

    public class CannotToRefusePaymentException() : DomainException<CannotToRefusePaymentException>("Only payments in the 'Pending' state can be refused.");
    
    public class InvalidAmountException() : DomainException<InvalidAmountException>("The amount cannot have a value equal to zero or negative");

    public class PaymentNotFoundException() : DomainException<PaymentNotFoundException>("No payment was found with the specified ID");
    
    public class ErrorInPaymentProcessingException() : DomainException<ErrorInPaymentProcessingException>("An error occurred while processing the payment. Try again later.");
}