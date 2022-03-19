using common.entityframework;
using common.exception;
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

    public async Task AddClaimAsync(ApplicationUser user, Claim claim)
    {
        var entity = new IdentityUserClaim<string>()
        {
            UserId = user.Id,
            ClaimType = claim.Type,
            ClaimValue = claim.Value
        };

        await DbContext.AspNetUserClaims.AddAsync(entity);
    }

    public async Task ReplaceClaimAsync(ApplicationUser user, Claim existingClaim, Claim newClaim)
    {
        var entity = await FirstOrDefaultAsync(x => x.ClaimType == existingClaim.Type && x.ClaimValue == existingClaim.Value && x.UserId == user.Id);
        if (entity == null)
            throw new FailedServiceException("cannot update claim while entity is null");

        entity.ClaimValue = newClaim.Value;
        DbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task RemoveClaimAsync(ApplicationUser user, Claim existingClaim)
    {
        var entity = await FirstOrDefaultAsync(x => x.ClaimType == existingClaim.Type && x.ClaimValue == existingClaim.Value && x.UserId == user.Id);
        if (entity == null)
            throw new FailedServiceException("cannot remove claim while entity is null");

        Delete(entity);
    }
}
