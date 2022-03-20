using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transaction.application.Features.Transactions.Commands.Deposit;

public static class AcceptedCent
{
    public static int[] cents = new int[] { 5, 10, 20, 50, 100 };
    public static bool CheckCent(int value)
    {
        if (cents.Any(c => c == value))        
            return true;            

        return false;
    }
}