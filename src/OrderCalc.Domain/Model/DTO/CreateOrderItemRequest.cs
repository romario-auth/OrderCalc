namespace OrderCalc.Domain.Model.DTO;

public class CreateOrderItemRequest
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
}