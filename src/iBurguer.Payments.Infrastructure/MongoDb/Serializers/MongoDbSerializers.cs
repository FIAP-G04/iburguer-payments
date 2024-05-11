using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace iBurguer.Payments.Infrastructure.MongoDb.Serializers;

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