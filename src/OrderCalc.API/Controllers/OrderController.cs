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
    
    public OrderController(IOrderServiceAplication orderServiceAplication)
    {
        _orderServiceAplication = orderServiceAplication;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id, CancellationToken cancellationToken)
    {
        var order = await _orderServiceAplication.Get(id, cancellationToken);

        if (order == null)
            return NotFound(new { message = $"Pedido com o pedidoId {id} não encontrado." });

        return Ok(order);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (request == null || request.Itens == null || !request.Itens.Any())
            return BadRequest(new { message = "Payload de pedido inválido." });

        try
        {
            var createdOrder = await _orderServiceAplication.Create(request, cancellationToken);

            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.PedidoId}, createdOrder);
        }
        catch (Exception ex)
        {
            // Aqui você pode logar o erro com Serilog, se já estiver configurado
            return StatusCode(500, new { message = "Ocorreu um erro ao criar o pedido.", details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetOrdersByStatus([FromQuery] string status, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _orderServiceAplication.GetByStatus(status, cancellationToken);

            if (orders == null || !orders.Any())
                return NotFound(new { message = $"Nenhum pedido encontrado com o status informado '{status}'." });

            return Ok(orders);
        }
        catch (Exception ex)
        {
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
