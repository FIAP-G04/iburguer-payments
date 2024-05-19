using AutoFixture.Xunit2;
using FluentAssertions;
using iBurguer.Payments.API.Controllers;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.UseCases.GenerateQrCode;
using iBurguer.Payments.Infrastructure.MongoDb.Serializers;
using iBurguer.Payments.UnitTests.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NSubstitute;
using NSubstitute.Exceptions;

namespace iBurguer.Payments.UnitTests.Infrastructure.MongoDbSerializers;

public class AmountSerializerTests : BaseTests
{
    public readonly PaymentController _sut;

    public AmountSerializerTests()
    {
        _sut = new PaymentController();
    }
    
    [Theory, AutoData]
    public async Task xxxx(GenerateQrCodeRequest request, GenerateQrCodeResponse response)
    {
        // Arrange
        var useCase = Substitute.For<IGenerateQrCodeUseCase>();
        useCase.GenerateQrCode(request, CancellationToken.None).Returns(response);

        // Act
        var result = await _sut.GenerateQrCode(useCase, request, CancellationToken.None) as CreatedResult;

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.Value.Should().BeOfType<GenerateQrCodeResponse>();
        result.Value.Should().BeEquivalentTo(response);
    }
}