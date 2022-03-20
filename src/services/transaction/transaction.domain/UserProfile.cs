using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transaction.domain;

public class UserProfile
{
    public string Id { get; set; }
    public string Email { get; set; }
    public double Deposit { get; set; }
    public string Role { get; set; }
}
