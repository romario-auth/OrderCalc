using Microsoft.AspNetCore.Mvc;
using OrderCalc.Application.Interfaces;
using OrderCalc.Application.Model.DTO;

namespace OrderCalc.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedido")]
public class OrderController : ControllerBase
{
    private readonly IOrderServiceAplication _orderServiceAplication;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderServiceAplication orderServiceAplication, ILogger<OrderController> logger)
    {
        _orderServiceAplication = orderServiceAplication;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id, CancellationToken cancellationToken)
    {
        var order = await _orderServiceAplication.Get(id, cancellationToken);
        _logger.LogInformation("Buscando pedido com ID: {Id}", id);

        if (order == null)
            return NotFound(new { message = $"Pedido com o pedidoId {id} não encontrado." });

        return Ok(order);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (request == null || request.Itens == null || !request.Itens.Any())
        {
            _logger.LogWarning("Tentativa de criar pedido com payload inválido.");
            return BadRequest(new { message = "Payload de pedido inválido." });
        }

        try
        {
            _logger.LogInformation("Iniciando criação de pedido para o cliente ID: {ClienteId}", request.ClienteId);
            var createdOrder = await _orderServiceAplication.Create(request, cancellationToken);
            _logger.LogInformation("Pedido criado com sucesso. PedidoId: {PedidoId}", createdOrder.PedidoId);

            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.PedidoId }, createdOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar o pedido para o cliente ID: {ClienteId}", request.ClienteId);
            return StatusCode(500, new { message = "Ocorreu um erro ao criar o pedido.", details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetOrdersByStatus([FromQuery] string status, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Buscando pedidos com status: {Status}", status);
            var orders = await _orderServiceAplication.GetByStatus(status, cancellationToken);

            if (orders == null || !orders.Any())
            {
                _logger.LogWarning("Nenhum pedido encontrado com o status: {Status}", status);
                return NotFound(new { message = $"Nenhum pedido encontrado com o status informado '{status}'." });
            }

            _logger.LogInformation("{Count} pedidos encontrados com o status: {Status}", orders.Count, status);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pedidos com o status: {Status}", status);
            return StatusCode(500, new { message = "Ocorreu um erro ao buscar os pedidos.", details = ex.Message });
        }
    }

    /// <summary>
    /// Dispose Controller
    /// </summary>
    [NonAction]
    public void Dispose()
    {
        _orderServiceAplication.Dispose();
        GC.SuppressFinalize(this);
    }
}
