using iBurguer.Payments.Core.Domain;
using MongoDB.Bson.Serialization;

namespace iBurguer.Payments.Infrastructure.MongoDb.Serializers;

public class PaymentStatusSerializer : IBsonSerializer<PaymentStatus>
{
    public Type ValueType => typeof(PaymentStatus);
    
    public PaymentStatus Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = context.Reader.ReadString();
        return PaymentStatus.FromName(value);
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, PaymentStatus value)
    {
        context.Writer.WriteString(value.Name);
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        if (value is PaymentStatus status)
        {
            Serialize(context, args, status);
        }
        else
        {
            throw new NotSupportedException("This is invalid payment status");
        }
    }

    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return Deserialize(context, args);
    }
}