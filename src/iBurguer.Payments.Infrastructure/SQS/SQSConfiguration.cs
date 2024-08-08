using System.Diagnostics.CodeAnalysis;

namespace iBurguer.Payments.Infrastructure.SQS
{
    [ExcludeFromCodeCoverage]
    public class SQSConfiguration
    {
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
