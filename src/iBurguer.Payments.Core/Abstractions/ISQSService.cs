namespace iBurguer.Payments.Core.Abstractions
{
    public interface ISQSService
    {
        Task SendMessage(IDomainEvent domainEvent, string queue, CancellationToken cancellationToken);
    }
}
