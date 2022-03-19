using MediatR;
using System.ComponentModel.DataAnnotations;

namespace user.application.Features.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest
{
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
    [Compare("Password", ErrorMessage = "Password and ConfirmPassword do not match.")]
    public string ConfirmPassword { get; set; }
    public double Deposit { get; set; }
    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; }
}
