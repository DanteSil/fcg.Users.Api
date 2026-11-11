using System.Text.Json;
using Fcg.Application.Users;
using RabbitMQ.Client;

namespace Fcg.Infrastructure.Messaging;

public sealed class RabbitMqUserCreatedPublisher(
    IConfiguration configuration,
    ILogger<RabbitMqUserCreatedPublisher> logger) : IUserCreatedPublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task PublishAsync(UserCreatedEvent userCreated)
    {
        var uri = configuration["RabbitMQ:Uri"];
        if (string.IsNullOrWhiteSpace(uri))
        {
            logger.LogWarning("RabbitMQ:Uri ausente. Evento de cadastro não publicado para {UserId}", userCreated.UserId);
            return;
        }

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var cancellationToken = timeout.Token;

        try
        {
            var factory = new ConnectionFactory { Uri = new Uri(uri), ClientProvidedName = "fcg-users" };
            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            var options = new CreateChannelOptions(publisherConfirmationsEnabled: true, publisherConfirmationTrackingEnabled: true);
            await using var channel = await connection.CreateChannelAsync(options, cancellationToken);
            await channel.ExchangeDeclareAsync("usuarios", ExchangeType.Direct, durable: true, cancellationToken: cancellationToken);
            await channel.QueueDeclareAsync("usuarios.notificacoes", durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await channel.QueueBindAsync("usuarios.notificacoes", "usuarios", "usuario.criado", cancellationToken: cancellationToken);

            var properties = new BasicProperties
            {
                ContentType = "application/json",
                Persistent = true,
                MessageId = userCreated.UserId.ToString(),
                Type = nameof(UserCreatedEvent)
            };

            var body = JsonSerializer.SerializeToUtf8Bytes(userCreated, JsonOptions);
            await channel.BasicPublishAsync("usuarios", "usuario.criado", mandatory: true,
                basicProperties: properties, body: body, cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Falha ao publicar o cadastro do usuário {UserId}", userCreated.UserId);
        }
    }
}
