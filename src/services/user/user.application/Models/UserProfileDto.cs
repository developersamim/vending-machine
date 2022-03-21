using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user.application.Models;

public class UserProfileDto
{
    public string Email { get; set; }
    public string Role { get; set; }
    public double Deposit { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
}
