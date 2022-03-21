using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace common.exception;

public class ApiError
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Detail { get; set; }

    public ApiError(int statusCode, string message, string detail = null)
    {
        StatusCode = statusCode;
        Message = message;
        Detail = detail;
    }

    public ApiError()
    {
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}