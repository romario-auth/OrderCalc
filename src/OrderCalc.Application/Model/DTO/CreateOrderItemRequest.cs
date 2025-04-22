namespace OrderCalc.Application.Model.DTO;

public class CreateOrderItemRequest
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
}