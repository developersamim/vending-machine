using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transaction.application.Features.Transactions.Commands.Buy;

public class BuyCommand : IRequest<BuyDto>
{
    public string ProductId { get; set; }
    public int Amount { get; set; }

    public string UserId { get; set; }
}
