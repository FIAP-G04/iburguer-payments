using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace iBurguer.Payments.Infrastructure.SQS.Events
{
    [ExcludeFromCodeCoverage]
    public class OrderRegisteredDomainEvent
    {
        [JsonProperty("orderId")]
        public Guid OrderId { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
