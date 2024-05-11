using iBurguer.Payments.Core.Domain;
using MongoDB.Bson.Serialization;

namespace iBurguer.Payments.Infrastructure.MongoDb.Serializers;

public class AmountSerializer : IBsonSerializer<Amount>
{
    public Type ValueType => typeof(Amount);
    
    public Amount Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = Convert.ToDecimal(context.Reader.ReadDouble());
        return new Amount(value);
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Amount amount)
    {
        context.Writer.WriteDouble(Convert.ToDouble(amount.Value));
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        if (value is Amount amount)
        {
            Serialize(context, args, amount);
        }
        else
        {
            throw new NotSupportedException("This is invalid amount");
        }
    }

    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return Deserialize(context, args);
    }
}