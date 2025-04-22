namespace OrderCalc.Application.Model.DTO;

public class OrderItemResponse
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
    public OrderItemResponse() {}
    public OrderItemResponse(int produtoId, int quantidade, decimal valor)
    {
        ProdutoId = produtoId;
        Quantidade = quantidade;
        Valor = valor;
    }
}