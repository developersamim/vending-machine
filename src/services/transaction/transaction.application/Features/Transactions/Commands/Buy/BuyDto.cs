using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transaction.application.Features.Transactions.Commands.Buy;

public class BuyDto
{
    public double Spent { get; set; }
    public ProductDto Product { get; set; }
}
