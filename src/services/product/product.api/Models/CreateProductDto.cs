using System.ComponentModel.DataAnnotations;

namespace product.api.Models;

public class CreateProductDto
{
    [Required(ErrorMessage = "Product name is required")]
    public string ProductName { get; set; }
    public int AmountAvailable { get; set; }
    public double Cost { get; set; }
}
