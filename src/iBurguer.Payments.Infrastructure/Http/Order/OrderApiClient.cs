using iBurguer.Payments.Core.Gateways;
using Microsoft.Extensions.Logging;

namespace iBurguer.Payments.Infrastructure.Http.Order;

public class OrderApiClient : IOrderGateway
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public OrderApiClient(OrderApiConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        
        _baseUrl = configuration.Url;
        _httpClient = httpClientFactory.CreateClient();
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
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while trying to confirm an order.", ex);
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
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while trying to cancel an order.", ex);
        }
    }
}