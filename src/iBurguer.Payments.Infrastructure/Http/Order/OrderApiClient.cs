using iBurguer.Payments.Core.Gateways;
using Microsoft.Extensions.Logging;

namespace iBurguer.Payments.Infrastructure.Http.Order;

public class OrderApiClient : IOrderGateway
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly ILogger<OrderApiClient> _logger;

    public OrderApiClient(OrderApiConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<OrderApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(logger);
        
        _baseUrl = configuration.Url;
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }
    
    public async Task<bool> ConfirmOrder(Guid orderId, CancellationToken cancellationToken = default)
    {
        string requestUrl = $"{_baseUrl}/api/orders/{orderId}/confirmed";
        
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUrl, null, cancellationToken);

        try
        {
            response.EnsureSuccessStatusCode();

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e.InnerException);

            throw;
        }
    }

    public async Task<bool> CancelOrder(Guid orderId, CancellationToken cancellationToken = default)
    {
        string requestUrl = $"{_baseUrl}/api/orders/{orderId}/canceled";
        
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUrl, null, cancellationToken);

        try
        {
            response.EnsureSuccessStatusCode();

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e.InnerException);

            throw;
        }
    }
}