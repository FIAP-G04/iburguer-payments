using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using iBurguer.Payments.Core.Abstractions;
using iBurguer.Payments.Core.UseCases.GenerateQrCode;
using iBurguer.Payments.Infrastructure.SQS.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace iBurguer.Payments.Infrastructure.SQS.Worker
{
    [ExcludeFromCodeCoverage]
    public class OrderRegisteredWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private ISQSService _sqsService;
        private IGenerateQrCodeUseCase _generateQrCodeUseCase;
        private readonly string _orderRegisteredQueue = "OrderRegistered";

        public OrderRegisteredWorker(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        protected async Task Handle(Message msg, CancellationToken cancellationToken)
        {
            var message = JsonConvert.DeserializeObject<OrderRegisteredDomainEvent>(msg.Body);

            var request = new GenerateQrCodeRequest()
            {
                OrderId = message.OrderId,
                Amount = message.Amount,
            };

            await _generateQrCodeUseCase.GenerateQrCode(request, cancellationToken);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var configuration = _configuration.GetRequiredSection("MassTransit").Get<SQSConfiguration>();

            using IServiceScope serviceScope = _scopeFactory.CreateScope();

            _generateQrCodeUseCase = serviceScope.ServiceProvider.GetRequiredService<IGenerateQrCodeUseCase>();
            _sqsService = serviceScope.ServiceProvider.GetRequiredService<ISQSService>();

            var queueUrl = await _sqsService.GetQueueUrl(_orderRegisteredQueue);

            await Start(queueUrl, stoppingToken);
        }

        private async Task Start(string queueUrl, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Starting polling queue");

            while (!cancellationToken.IsCancellationRequested)
            {
                var messages = await _sqsService.ReceiveMessageAsync(queueUrl);

                if (messages.Any())
                {
                    Console.WriteLine($"{messages.Count()} messages received");

                    foreach (var msg in messages)
                    {
                        await Handle(msg, cancellationToken);
                        await _sqsService.DeleteMessageAsync(queueUrl, msg.ReceiptHandle);
                    }
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
        }
    }
}
