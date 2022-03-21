using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace user.domain;

public class UserProfile
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("role")]
    public string Role { get; set; }
    [JsonPropertyName("deposit")]
    public double Deposit { get; set; }
    [JsonPropertyName("givenName")]
    public string GivenName { get; set; }
    [JsonPropertyName("familyName")]
    public string FamilyName { get; set; }
}
