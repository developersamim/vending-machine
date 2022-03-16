using System;
namespace product.api.Services;

public interface IHttpContextAccessorService
{
    string GetUserCode();
    string GetAccessToken();
}

