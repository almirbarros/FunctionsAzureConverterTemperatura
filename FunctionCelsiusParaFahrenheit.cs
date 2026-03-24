using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ConversaoTemperatura
{
    public class FunctionCelsiusParaFahrenheit
    {
        private readonly ILogger _logger;

        public FunctionCelsiusParaFahrenheit(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FunctionCelsiusParaFahrenheit>();
        }

        [Function("ConverterCelsiusParaFahrenheit")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Conversão" })]
        [OpenApiParameter(name: "celsius", In = ParameterLocation.Path, Required = true, Type = typeof(double), Description = "Temperatura em Celsius")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "Retorna o valor em Fahrenheit")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ConverterCelsiusParaFahrenheit/{celsius}")] HttpRequestData req, double celsius)
        {
            _logger.LogInformation("Convertendo Celsius {celsius} para Fahrenheit.", celsius);

            var valorfahrenheit = (celsius * 9 / 5) + 32;

            var resultado = new
            {
                celsius = celsius,
                fahrenheit = valorfahrenheit,
                mensagem = $"O valor {celsius}°C convertido para Fahrenheit é {valorfahrenheit}°F"
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            // Use WriteAsJsonAsync para garantir a serialização correta
            await response.WriteAsJsonAsync(resultado);

            _logger.LogInformation("Conversão efetuada. Resultado: {valorFahrenheit}", valorfahrenheit);

            return response;
        }
    }
}