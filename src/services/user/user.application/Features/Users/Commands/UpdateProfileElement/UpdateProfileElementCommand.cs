using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user.application.Features.Users.Commands.UpdateProfileElement;

public class UpdateProfileElementCommand : IRequest
{
    public string UserId { get; set; }
    public Dictionary<string, object> KeyValuePairs { get; set; }
}
