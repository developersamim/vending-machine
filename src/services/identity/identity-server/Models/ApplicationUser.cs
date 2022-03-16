using System;
using Microsoft.AspNetCore.Identity;

namespace identity_server.Models;

public class ApplicationUser : IdentityUser
{
	public ApplicationUser() : base() { }
	public ApplicationUser(string userName) : base(userName) { }
}

