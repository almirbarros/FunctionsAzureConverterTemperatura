// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Azure.Functions.Worker;
// using Microsoft.Extensions.Logging;

// namespace ConversaoTemperatura;

// public class ConversaoTemperatura
// {
//     private readonly ILogger<ConversaoTemperatura> _logger;

//     public ConversaoTemperatura(ILogger<ConversaoTemperatura> logger)
//     {
//         _logger = logger;
//     }

//     [Function("ConversaoTemperatura")]
//     public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
//     {
//         _logger.LogInformation("C# HTTP trigger function processed a request.");
//         return new OkObjectResult("Welcome to Azure Functions!");
//     }
// }

using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

//namespace Empresa.Produtos
namespace ConversaoTemperatura
{
    // public class ProdutoFunc
    // {
    //     private readonly ILogger _logger;

    //     public ProdutoFunc(ILoggerFactory loggerFactory)
    //     {
    //         _logger = loggerFactory.CreateLogger<ProdutoFunc>();
    //     }

    //     [Function("GetProdutos")]
    //     [OpenApiOperation(operationId: "Run", tags: new[] { "Produtos" })]
    //     [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    //     [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<string>), Description = "A lista de produtos")]
    //     public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    //     {
    //         _logger.LogInformation("Listando produtos.");

    //         var produtos = new List<string> { "Produto A", "Produto B" };
    //         var response = req.CreateResponse(HttpStatusCode.OK);
    //         await response.WriteAsJsonAsync(produtos);

    //         return response;
    //     }
    // }

    public class FunctionFahrenheitParaCelsius
    {
        private readonly ILogger _logger;

        public FunctionFahrenheitParaCelsius(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FunctionFahrenheitParaCelsius>();
        }

        [Function("ConverterFahrenheitParaCelsius")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Conversão" })]
        // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "fahrenheit", In = ParameterLocation.Path, Required = true, Type = typeof(double), Description = "Temperatura em Fahrenheit")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "Retorna o valor em Celsius")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ConverterFahrenheitParaCelsius/{fahrenheit}")] HttpRequestData req, double fahrenheit)
        {
            _logger.LogInformation("Convertendo Fahrenheit {fahrenheit} para Celsius.", fahrenheit);

            var valorCelsius = (fahrenheit - 32) * 5 / 9;

            var resultado = new
            {
                fahrenheit = fahrenheit,
                celsius = valorCelsius,
                mensagem = $"O valor {fahrenheit}°F convertido para Celsius é {valorCelsius}°C"
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            // Use WriteAsJsonAsync para garantir a serialização correta
            await response.WriteAsJsonAsync(resultado);

            _logger.LogInformation("Conversão efetuada. Resultado: {valorCelsius}", valorCelsius);

            return response;
        }
    }
}