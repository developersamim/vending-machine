using transaction.domain;

namespace transaction.application.Contracts.Infrastructure;

public interface IUserService
{
    Task<UserProfile> GetProfile(string userId);
    Task UpdateProfile(string userId, Dictionary<string, object> userProfile);
}
