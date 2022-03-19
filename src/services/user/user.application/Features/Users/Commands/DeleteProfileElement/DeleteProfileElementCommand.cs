using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user.application.Features.Users.Commands.DeleteProfileElement;

public class DeleteProfileElementCommand : IRequest
{
    public string UserId { get; set; }
    public List<string> Keys { get; set; }
}
