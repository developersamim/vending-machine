using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transaction.application.Features.Transactions.Commands.Deposit;

public class DepositCommand : IRequest
{
    public int Cent { get; set; }
    public string UserId { get; set; }
}
