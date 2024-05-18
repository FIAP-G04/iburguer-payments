using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace iBurguer.Payments.Infrastructure.MongoDb.Serializers;

[ExcludeFromCodeCoverage]
public static class MongoDbSerializers
{
    public static void Register()
    {
        BsonSerializer.TryRegisterSerializer(new ObjectSerializer(ObjectSerializer.AllAllowedTypes));
        BsonSerializer.TryRegisterSerializer(GuidSerializer.StandardInstance);
        BsonSerializer.TryRegisterSerializer(new AmountSerializer());
        BsonSerializer.TryRegisterSerializer(new PaymentStatusSerializer());
    }
}