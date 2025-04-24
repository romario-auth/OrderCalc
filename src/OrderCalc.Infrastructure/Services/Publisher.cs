using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderCalc.Domain.Interfaces;
using OrderCalc.Domain.Settings;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


namespace OrderCalc.Infrastructure.Services;

public class Publisher : IPublisher
{
    private readonly RabbitMQSettings _settings;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<Publisher> _logger;

    public Publisher(IOptions<RabbitMQSettings> options, ILogger<Publisher> logger)
    {
        _settings = options.Value;

        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            VirtualHost = _settings.VirtualHost,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        _logger = logger;

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare Exchange
        _channel.ExchangeDeclare(
            exchange: _settings.DefaultExchangeName,
            type: _settings.DefaultExchangeType,
            durable: true,
            autoDelete: false
        );

        // Declare queue:
        // To do: abstract to any queue
        _channel.QueueDeclare(
            queue: "order.queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        // Binding to queue
        // To do: abstract to any queue
        _channel.QueueBind(
            queue: "order.queue",
            exchange: _settings.DefaultExchangeName,
            routingKey: "order.created"
        );
    }

    public void Publish<T>(T message, string routingKey)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: _settings.DefaultExchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: body
            );

            _logger.LogInformation("Mensagem publicada com sucesso. RoutingKey: {RoutingKey}, Payload: {Payload}", routingKey, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar mensagem. RoutingKey: {RoutingKey}, Payload: {Payload}", routingKey, message);
        }
    }


}
