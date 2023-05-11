using Grpc.Core;
using GrpcTest.Clients;
using GrpcTest.Translator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcTest.Net7WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly GrpcTestTranslator.GrpcTestTranslatorClient _grpcClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, GrpcTestTranslator.GrpcTestTranslatorClient grpcClient)
        {
            _logger = logger;
            _grpcClient = grpcClient;
        }

        [HttpGet]
        [Route("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("GrpcTest")]
        public async Task<string> GrpcTest()
        {
            try
            {
                var res = await _grpcClient.TranslateOneValueAsync(new TranslateOneValueRequest() { Value = "vasya", Section = "section1" });
                return res.Value;
            }
            catch (RpcException ex)
            {
                if (!string.IsNullOrWhiteSpace(ex.Status.Detail))
                    return "RpcException: " + ex.Status.Detail;
                else
                    throw;
            }
        }

        [HttpGet]
        [Route("Test2")]
        public async Task<string> Test2()
        {
            return MicroservicesClientComposition.TryGetFrameworkDescription();
        }
    }
}