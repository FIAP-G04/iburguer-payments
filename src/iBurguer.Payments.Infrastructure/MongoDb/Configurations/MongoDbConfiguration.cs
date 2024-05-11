namespace iBurguer.Payments.Infrastructure.MongoDb.Configurations;

public record MongoDbConfiguration
{
    public required string ConnectionString { get; set; }
    public required string Database { get; set; }

    public void ThrowIfInvalid()
    {
        if (string.IsNullOrEmpty(ConnectionString)
            || string.IsNullOrEmpty(Database))
        {
            throw new InvalidOperationException("Falha no carregamento das informações necessárias para configuração do MongoDB.");
        }
    }
}