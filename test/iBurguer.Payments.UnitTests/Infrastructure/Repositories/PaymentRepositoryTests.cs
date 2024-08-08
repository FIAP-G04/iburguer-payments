using AutoFixture.Xunit2;
using FluentAssertions;
using iBurguer.Payments.Core.Abstractions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Infrastructure.EventDispatcher;
using iBurguer.Payments.Infrastructure.MongoDb;
using iBurguer.Payments.Infrastructure.Repositories;
using iBurguer.Payments.UnitTests.Util;
using MongoDB.Driver;
using NSubstitute;

namespace iBurguer.Payments.UnitTests.Infrastructure.Repositories;

public class PaymentRepositoryTests : BaseTests
{
    private readonly PaymentRepository _sut;
    private readonly IMongoCollection<Payment> _mockCollection;
    private readonly IDbContext _mockContext;
    private readonly IEventDispatcher _mockDispatcher;

    public PaymentRepositoryTests()
    {
        _mockCollection = Substitute.For<IMongoCollection<Payment>>();
        _mockContext = Substitute.For<IDbContext>();
        _mockDispatcher = Substitute.For<IEventDispatcher>();

        _mockContext.Database.GetCollection<Payment>("payments").Returns(_mockCollection);
        _sut = new PaymentRepository(_mockContext, _mockDispatcher);
    }

    [Theory, AutoData]
    public async Task ShouldDispatchEvents(Guid orderId, decimal amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);

        payment.Confirm();

        // Act
        var result = await _sut.Update(payment, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        await _mockDispatcher.Received(1).Dispatch(Arg.Any<IDomainEvent>(), CancellationToken.None);
    }

    [Theory, AutoData]
    public async Task ShouldSavePayment(Guid orderId, decimal amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);

        // Act
        await _sut.Save(payment);

        // Assert
        await _mockCollection.Received(1).InsertOneAsync(payment, null);
    }

    [Theory, AutoData]
    public async Task ShouldRollbackTransactionOnUpdateFailure(Guid orderId, decimal amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        payment.Confirm();
        var cancellationToken = new CancellationToken();

        var mockSession = Substitute.For<IClientSessionHandle>();
        _mockContext.CreateSession().Returns(Task.FromResult(mockSession));
        _mockCollection.When(x => x.UpdateOneAsync(mockSession,
            Arg.Any<FilterDefinition<Payment>>(),
            Arg.Any<UpdateDefinition<Payment>>(),
            null,
            cancellationToken))
            .Do(x => { throw new Exception("Update failed"); });

        // Act
        var result = await _sut.Update(payment, cancellationToken);

        // Assert
        result.Should().BeFalse();
        await _mockContext.Received(1).Rollback(mockSession);
    }
}