namespace transaction.api.Models;

public class UpdateProductDto
{
    public string? ProductName { get; set; }
    public int? AmountAvailable { get; set; }
    public double? Cost { get; set; }
}
