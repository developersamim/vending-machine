using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace common.exception;

public class FailedServiceException : HttpRequestException
{
    public FailedServiceException(string message) : base(message, null, 0)
    { }

    public FailedServiceException(HttpResponseMessage response) : base(response.ReasonPhrase, null, response.StatusCode)
    {
    }
    public FailedServiceException(HttpResponseMessage response, Exception innerException) : base(response.ReasonPhrase, innerException, response.StatusCode)
    {
    }
    public FailedServiceException(HttpResponseMessage response, string message) : base(message ?? response.ReasonPhrase, null, response.StatusCode)
    {
    }
    public FailedServiceException(HttpResponseMessage response, string message, Exception innerException) : base(message ?? response.ReasonPhrase, innerException, response.StatusCode)
    {
    }
    public FailedServiceException(string message, HttpStatusCode statusCode) : base(message, null, statusCode)
    {
    }
}
