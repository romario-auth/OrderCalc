namespace OrderCalc.Application.Model.DTO;

public class CreateOrderRequest
{
    public int PedidoId { get; set; }
    public int ClienteId { get; set; }
    public List<CreateOrderItemRequest> Itens { get; set; } = new();
}