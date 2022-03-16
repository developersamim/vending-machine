using common.entityframework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using user.application.Contracts.Persistence;
using user.domain;
using user.infrastructure.Persistence;

namespace user.infrastructure.Repositories;

public class UserClaimRepository : BaseRepository<IdentityUserClaim<string>, ApplicationDbContext>, IUserClaimRepository
{
    public UserClaimRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {

    }
    public async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        Expression<Func<IdentityUserClaim<string>, bool>> predicate
                = uc => uc.UserId == user.Id;

        var dbResults = await DbContext.AspNetUserClaims
            .Where(predicate)
            .Select(uc => new { uc.UserId, claim = uc.ToClaim() })
            .ToListAsync();

        return dbResults.Select(c => c.claim).ToList();
    }
}
