using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using serverless.Infrastructure;

namespace product_function
{
    public class product
    {
        private readonly ILogger _logger;
        private readonly IProductService productService;

        public product(ILoggerFactory loggerFactory, IProductService productService)
        {
            _logger = loggerFactory.CreateLogger<product>();
            this.productService = productService;
        }

        [Function("product")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Admin, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var result = await productService.GetProducts();
            return new OkObjectResult(result);

            //var response = req.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            //response.WriteString("Welcome to Azure Functions!");

            //return response;

            
        }
    }
}
