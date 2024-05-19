namespace iBurguer.Payments.Core.Abstractions;

public abstract class DomainException<TException>(string message) : 
    ApplicationException(message) where TException : DomainException<TException>, new()
{
    public static void ThrowIf(bool condition)
    {
        if (condition) throw new TException();
    }
    
    public static void ThrowIfNull<T>(T t)
    {
        if (t is null) throw new TException();
    }
}