using System;
namespace transaction.api.Services;

public interface IHttpContextAccessorService
{
    string GetUserCode();
    string GetAccessToken();
}

