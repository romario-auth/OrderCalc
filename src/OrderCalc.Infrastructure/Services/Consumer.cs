using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderCalc.Domain.Interfaces;
using OrderCalc.Domain.Interfaces.Services;
using OrderCalc.Domain.Model.DTO;
using OrderCalc.Domain.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderCalc.Infrastructure.Services;

public class Consumer : IConsumer
{
    private readonly ILogger<Consumer> _logger;
    private readonly RabbitMQSettings _settings;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IOrderService _orderService;

    public Consumer(IOptions<RabbitMQSettings> options, ILogger<Consumer> logger, IOrderService orderService)
    {
        _settings = options.Value;
        _logger = logger;
        _orderService = orderService;

        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            VirtualHost = _settings.VirtualHost,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Garante que a queue está criada
        _channel.QueueDeclare(
            queue: "order.queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        // Garante que a binding está feita
        _channel.QueueBind(
            queue: "order.queue",
            exchange: _settings.DefaultExchangeName,
            routingKey: "order.created"
        );
    }

    public Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var orderCreated = JsonSerializer.Deserialize<OrderCreatedMessage>(json);

            if (orderCreated == null)
            {
                _logger.LogError("Falha ao desserializar OrderCreatedMessage: {Json}", json);
                return;
            }


            _logger.LogInformation($"[RabbitMQ] Mensagem recebida: {json}");

            await _orderService.CalculateTaxAsync(orderCreated.OrderId, cancellationToken);

            // Após processar, confirma o recebimento da mensagem
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicQos(0, 1, false);

        _channel.BasicConsume(
            queue: "order.queue",
            autoAck: false,
            consumer: consumer
        );

        return Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }, cancellationToken);
    }
}
