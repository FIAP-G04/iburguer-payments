using FluentAssertions;
using iBurguer.Payments.Core.Abstractions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Infrastructure.EventDispatcher;
using NSubstitute;

namespace iBurguer.Payments.UnitTests.Infrastructure.EventDispatchers;

public class EventDispatcherTests
{
    [Fact]
    public async Task ShouldInvokeHandler()
    {
        // Arrange
        var serviceProvider = Substitute.For<IServiceProvider>();
        var eventHandler = Substitute.For<IEventHandler<PaymentConfirmed>>();
        serviceProvider.GetService(typeof(IEventHandler<PaymentConfirmed>)).Returns(eventHandler);
        var dispatcher = new EventDispatcher(serviceProvider);
        var testEvent = new PaymentConfirmed(Guid.NewGuid());

        // Act
        await dispatcher.Dispatch(testEvent, CancellationToken.None);

        // Assert
        await eventHandler.Received(1).Handle(testEvent, CancellationToken.None);
    }

    [Fact]
    public void ShouldThrowInvalidOperationExceptionWhenHandlerNotFound()
    {
        // Arrange
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(Arg.Any<Type>()).Returns((object)null);
        var dispatcher = new EventDispatcher(serviceProvider);
        var testEvent = new PaymentConfirmed(Guid.NewGuid());

        // Act
        Func<Task> action = async () => await dispatcher.Dispatch(testEvent, CancellationToken.None);

        // Assert
        action.Should().ThrowAsync<InvalidOperationException>();
    }
}