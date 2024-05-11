using Ardalis.SmartEnum;

namespace iBurguer.Payments.Core.Domain;


public class PaymentStatus : SmartEnum<PaymentStatus>
{
    public static readonly PaymentStatus Pending = new("Pending", 1);
    public static readonly PaymentStatus Processed = new("Processed", 2);
    public static readonly PaymentStatus Refused = new("Refused", 3);

    private PaymentStatus(string name, int value) : base(name, value) { }
}