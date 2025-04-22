namespace OrderCalc.Application.Model.DTO;

public class OrderResponse
{
    public int PedidoId { get; set; }
    public int ClienteId { get; set; }
    public decimal Imposto { get; set; }
    public string Status { get; set; }
    public List<OrderItemResponse> Itens { get; set; }
    public OrderResponse()
    {
        Status = "";
        Itens = new List<OrderItemResponse>(){};
    }

    public OrderResponse(int pedidoId, int clienteId, decimal imposto, string status, List<OrderItemResponse> orderItemResponse)
    {
        PedidoId = pedidoId;
        ClienteId = clienteId;
        Imposto = imposto;
        Status = status;
        Itens = orderItemResponse;
    }
}