using ActiveMQ_PoC.Shared.Constants;
using ActiveMQ_PoC.Shared.Interfaces;
using ActiveMQ_PoC.Shared.Interfaces.Commands;
using ActiveMQ_PoC.Shared.Interfaces.Events;
using ActiveMQ_PoC.Shared.Interfaces.Requests;
using Bogus;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace ActiveMQ_PoC.WebApp.Controllers
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
        private readonly IRequestClient<IGetTransportOrderStatusRequest> _requestClient;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRequestClient<IGetTransportOrderStatusRequest> requestClient, IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _requestClient = requestClient;
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("Request")]
        public async Task<ITransportOrderResponse> RequestExample(string referenceId)
        {
            var result = await _requestClient.GetResponse<ITransportOrderResponse>(new {ReferenceId = referenceId});
            return result.Message;
        }

        [HttpPost("Publish")]
        public async Task Publish(string referenceId)
        {
            var faker = new Faker();
            await _publishEndpoint.Publish<ITransportOrderAmendedEvent>(new
            {
                ReferenceId = referenceId,
                DatabaseId = faker.Random.Int(1, 999),
                EventDateTime = faker.Date.Future(),
                BlobUrl = faker.Internet.Url()
            });
        }

        [HttpPost("Send")]
        public async Task Send(string referenceId)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{QueueName.ConsumerA}"));
            var faker = new Faker();
            await endpoint.Send<ITransportOrderAmendedEvent>(new
            {
                ReferenceId = referenceId,
                DatabaseId = faker.Random.Int(1, 999),
                EventDateTime = faker.Date.Future(),
                BlobUrl = faker.Internet.Url()
            });
        }

        [HttpPost("Command")]
        public async Task Command(string referenceId)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{QueueName.ConsumerB}"));
            var faker = new Faker();
            await endpoint.Send<ICreateTransportOrderCommand>(new
            {
                ReferenceId = referenceId,
                DatabaseId = faker.Random.Int(1, 999),
                EventDateTime = faker.Date.Future(),
                BlobUrl = faker.Internet.Url(),
                TransportOrderNumber = referenceId
            });
        }
    }
}