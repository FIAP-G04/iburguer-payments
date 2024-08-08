using System.Net;
using System.Text;
using AutoFixture.Xunit2;
using FluentAssertions;
using iBurguer.Payments.Infrastructure.Http.Order;
using iBurguer.Payments.UnitTests.Util;
using Moq;
using Moq.Protected;

namespace iBurguer.Payments.UnitTests.Infrastructure.Http;

public class OrderApiClientTest
{
    private readonly OrderApiConfiguration _configuration;
    private readonly Mock<HttpMessageHandler> _handlerMock;
    private readonly HttpClient _httpClient;
    private readonly OrderApiClient _sut;

    public OrderApiClientTest()
    {
        _configuration = new OrderApiConfiguration { Url = "https://example.com" };

        _handlerMock = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri(_configuration.Url)
        };
        
        _sut = new OrderApiClient(_configuration, new HttpClientFactoryMock(_httpClient));
    }

    [Theory, AutoData]
    public async Task ShouldConfirmOrderSuccessfully(Guid orderId)
    {
        //Arrange
        MockMockReturnSuccessfully();
        
        // Act
        var result = await _sut.ConfirmOrder(orderId, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
    
    [Theory, AutoData]
    public async Task ShouldThrowAnExceptionWhenConfirmingARequestAndAFailureOccurs(Guid orderId)
    {
        //Arrange
        MockMockReturnFailure();
        
        // Act
        Func<Task> act = async () => await _sut.ConfirmOrder(orderId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
    
    [Theory, AutoData]
    public async Task ShouldCancelOrderSuccessfully(Guid orderId)
    {
        //Arrange
        MockMockReturnSuccessfully();
        
        // Act
        var result = await _sut.CancelOrder(orderId, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
    
    [Theory, AutoData]
    public async Task ShouldThrowAnExceptionWhenCancelingARequestAndAFailureOccurs(Guid orderId)
    {
        //Arrange
        MockMockReturnFailure();
        
        // Act
        Func<Task> act = async () => await _sut.CancelOrder(orderId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private void MockMockReturnSuccessfully()
    {
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("", Encoding.UTF8, "application/json")
            });
    }
    
    private void MockMockReturnFailure()
    {
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("", Encoding.UTF8, "application/json")
            });
    }
}