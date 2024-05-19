namespace iBurguer.Payments.UnitTests.Util;

public class HttpClientFactoryMock : IHttpClientFactory
{
    private readonly HttpClient _client;

    public HttpClientFactoryMock(HttpClient client)
    {
        _client = client;
    }

    public HttpClient CreateClient(string name = "")
    {
        return _client;
    }
}