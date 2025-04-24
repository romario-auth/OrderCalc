using OrderCalc.Domain.Interfaces;

namespace OrderCalc.Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConsumer _consumerService;

    public Worker(ILogger<Worker> logger, IConsumer consumerService)
    {
        _logger = logger;
        _consumerService = consumerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

        // Consome a fila de forma contínua
        await _consumerService.StartConsumingAsync(stoppingToken);
    }
}