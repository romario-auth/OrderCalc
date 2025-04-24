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
        // TODO: abstrair a implementação para qualquer fila
        _channel.QueueDeclare(
            queue: "order.queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        // Garante que a binding está feita
        // TODO: abstrair a implementação para qualquer fila/routingKey
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

            await Task.Delay(5000);

            await _orderService.CalculateTaxAsync(orderCreated.OrderId, cancellationToken);

            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

            // TODO: Enviar o pedido com imposto calculado para uma segunda fila, que será consumida pelo microserviço "Produto Externo B".
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
