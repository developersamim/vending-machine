using AutoMapper;
using common.exception;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace common.infrastructure;

public abstract class BaseService<T>
{
    protected ILogger<T> Logger { get; }

    protected HttpClient Client { get; }

    protected IMapper Mapper { get; set; }

    protected BaseService(ILogger<T> logger, HttpClient client, IMapper mapper)
    {
        Logger = logger;
        Client = client;
        Mapper = mapper;
    }

    public virtual async Task<TM> ValidateResponse<TM>(HttpResponseMessage response) where TM : class
    {
        if (!response.IsSuccessStatusCode)
            throw new FailedServiceException(response);

        var content = await response.Content.ReadFromJsonAsync<TM>();
        return content;

    }

    public virtual void ValidateResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new FailedServiceException(response);
    }

}
