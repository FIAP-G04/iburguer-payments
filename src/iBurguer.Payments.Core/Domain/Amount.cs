using System.Globalization;
using static iBurguer.Payments.Core.Exceptions;

namespace iBurguer.Payments.Core.Domain;

public sealed record Amount
{
    public decimal Value { get; init; } = 0;

    public Amount(decimal amount)
    {
        InvalidAmountException.ThrowIf(amount < 0);

        Value = amount;
    }

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    public static implicit operator decimal(Amount amount) => amount.Value;

    public static implicit operator Amount(decimal value) => new(value);
}