using common.entityframework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using user.domain;

namespace user.application.Contracts.Persistence;

public interface IUserClaimRepository : IAsyncRepository<IdentityUserClaim<string>>
{
    Task<List<Claim>> GetClaimsAsync(ApplicationUser user);
}
