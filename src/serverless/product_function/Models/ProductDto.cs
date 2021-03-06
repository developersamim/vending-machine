using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverless.Models;

public class ProductDto
{
    public string Id { get; set; }
    public string ProductName { get; set; }
    public int AmountAvailable { get; set; }
    public double Cost { get; set; }

    public string SellerId { get; set; }

    public bool ByPass { get; set; } = true;
}
