using System;
using IdentityModel.Client;

namespace mvc_app.Services;

public interface ITokenService
{
    Task<TokenResponse> GetToken(string scope);
}

