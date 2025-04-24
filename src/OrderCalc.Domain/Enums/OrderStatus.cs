using System.ComponentModel.DataAnnotations;

namespace OrderCalc.Domain.Enums;

public enum OrderStatus
{
    [Display(Name = "Criado")]
    Created = 0,

    [Display(Name = "Processando")]
    Processing = 1,

    [Display(Name = "Concluído")]
    Completed = 2,

    [Display(Name = "Cancelado")]
    Canceled = 3,
    
    [Display(Name = "Calculado")]
    Calculated = 4
}