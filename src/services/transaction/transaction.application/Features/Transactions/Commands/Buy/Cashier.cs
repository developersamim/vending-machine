using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using transaction.application.Features.Transactions.Commands.Deposit;

namespace transaction.application.Features.Transactions.Commands.Buy;

public static class Cashier
{
    public static List<double> GetChange(double totalCent)
    {
        var change = new List<double>();
        double temp = 0;
        do
        {
            foreach (var item in AcceptedCent.cents.Select((value, i) => new { i, value }))
            {
                var value = item.value;
                var index = item.i;

                Console.WriteLine($"Value: {value}, Index: {index}");

                if (value < totalCent && temp < totalCent)
                {
                    var anothertemp = temp + value;
                    if (anothertemp <= totalCent)
                    {
                        temp = temp + value;
                        change.Add(value);
                    }
                }
            }
        }
        while (temp < totalCent);

        return change;
    }
}
