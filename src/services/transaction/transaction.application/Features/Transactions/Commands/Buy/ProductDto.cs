using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transaction.application.Features.Transactions.Commands.Buy;

public class ProductDto
{
    public string Id { get; set; }
    public string ProductName { get; set; }
    public int AmountAvailable { get; set; }
    public double Cost { get; set; }
}
