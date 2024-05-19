using iBurguer.Payments.Infrastructure.MongoDb;
using iBurguer.Payments.Infrastructure.MongoDb.Configurations;
using MongoDB.Driver;
using Moq;

namespace iBurguer.Payments.UnitTests.Infrastructure.MongoDb;

public class DbContextTests
{
    [Fact]
    public async Task CreateSession_Should_Return_Session()
    {
        // Arrange
        var configuration = new MongoDbConfiguration { ConnectionString = "mongodb://localhost:27017", Database = "test" };
        var mockClient = new Mock<IMongoClient>();
        var mockDatabase = new Mock<IMongoDatabase>();
        mockClient.Setup(c => c.GetDatabase(configuration.Database, null)).Returns(mockDatabase.Object);
        var dbContext = new DbContext(configuration);

        // Act
        var session = await dbContext.CreateSession();

        // Assert
        Assert.NotNull(session);
    }

    [Fact]
    public void BeginTransaction_Should_Start_Transaction()
    {
        // Arrange
        var configuration = new MongoDbConfiguration { ConnectionString = "mongodb://localhost:27017", Database = "test" };
        var mockClient = new Mock<IMongoClient>();
        var mockDatabase = new Mock<IMongoDatabase>();
        var mockSession = new Mock<IClientSessionHandle>();
        mockClient.Setup(c => c.GetDatabase(configuration.Database, null)).Returns(mockDatabase.Object);
        var dbContext = new DbContext(configuration);

        // Act
        dbContext.BeginTransaction(mockSession.Object);

        // Assert
        mockSession.Verify(s => s.StartTransaction(null), Times.Once);
    }

    [Fact]
    public async Task Commit_Should_Commit_Transaction()
    {
        // Arrange
        var configuration = new MongoDbConfiguration { ConnectionString = "mongodb://localhost:27017", Database = "test" };
        var mockClient = new Mock<IMongoClient>();
        var mockDatabase = new Mock<IMongoDatabase>();
        var mockSession = new Mock<IClientSessionHandle>();
        mockClient.Setup(c => c.GetDatabase(configuration.Database, null)).Returns(mockDatabase.Object);
        var dbContext = new DbContext(configuration);

        // Act
        await dbContext.Commit(mockSession.Object);

        // Assert
        mockSession.Verify(s => s.CommitTransactionAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Rollback_Should_Rollback_Transaction()
    {
        // Arrange
        var configuration = new MongoDbConfiguration { ConnectionString = "mongodb://localhost:27017", Database = "test" };
        var mockClient = new Mock<IMongoClient>();
        var mockDatabase = new Mock<IMongoDatabase>();
        var mockSession = new Mock<IClientSessionHandle>();
        mockClient.Setup(c => c.GetDatabase(configuration.Database, null)).Returns(mockDatabase.Object);
        var dbContext = new DbContext(configuration);

        // Act
        await dbContext.Rollback(mockSession.Object);

        // Assert
        mockSession.Verify(s => s.AbortTransactionAsync(CancellationToken.None), Times.Once);
    }
}