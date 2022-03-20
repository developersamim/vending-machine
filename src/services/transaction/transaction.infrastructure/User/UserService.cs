using AutoMapper;
using common.infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using transaction.application.Contracts.Infrastructure;
using transaction.domain;

namespace transaction.infrastructure.User;

public class UserService : BaseService<IUserService>, IUserService
{
    private const string ControllerUrl = "user";

    public UserService(ILogger<IUserService> logger, HttpClient client, IMapper mapper)
        : base(logger, client, mapper) { }

    public async Task UpdateProfile(string userId, Dictionary<string, object> userProfile)
    {
        var queryParam = new Dictionary<string, string>
        {
            { "userId", userId }
        };
        var url = QueryHelpers.AddQueryString($"{ControllerUrl}", queryParam);
        var result = await Client.PutAsJsonAsync(url, userProfile);
        ValidateResponse(result);
    }

    public async Task<UserProfile> GetProfile(string userId)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "userId", userId }
        };
        var url = QueryHelpers.AddQueryString($"{ControllerUrl}", queryParams);
        var response = await Client.GetAsync(url);
        var result = await ValidateResponse<UserProfile>(response);
        return result;
    }
}
