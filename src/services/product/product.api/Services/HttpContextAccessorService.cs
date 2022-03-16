using System;
namespace product.api.Services;

public class HttpContextAccessorService : IHttpContextAccessorService
{
    private readonly IHttpContextAccessor context;

	public HttpContextAccessorService(IHttpContextAccessor context)
	{
        this.context = context;
	}

    public string GetAccessToken()
    {
        var accessToken = context.HttpContext.Request.Headers["Authorization"];

        return accessToken;
    }

    public string GetUserCode()
    {
        throw new NotImplementedException();
    }
}

