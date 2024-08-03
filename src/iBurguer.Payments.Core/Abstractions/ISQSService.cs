using Amazon.SQS.Model;

namespace iBurguer.Payments.Core.Abstractions
{
    public interface ISQSService
    {
        Task<string> GetQueueUrl(string queueName);
        Task<List<Message>> ReceiveMessageAsync(string queueUrl);
        Task DeleteMessageAsync(string queueUrl, string id);
        Task SendMessage(IDomainEvent domainEvent, string queue, CancellationToken cancellationToken);
    }
}
