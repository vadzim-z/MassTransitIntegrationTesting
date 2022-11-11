using ActiveMQ_PoC.Shared.Interfaces.Requests;
using Bogus;
using MassTransit;
using System.Text.Json;
using ActiveMQ_PoC.Shared.Entities;
using ActiveMQ_PoC.Shared.Interfaces.Repositories;

namespace ActiveMQ_PoC.WebApp.Consumers;

public class GetStatusConsumer : IConsumer<IGetTransportOrderStatusRequest>
{
    private readonly ITransportOrderRepository _repository;

    public GetStatusConsumer(ITransportOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<IGetTransportOrderStatusRequest> context)
    {
        var faker = new Faker();
        var response = new TransportOrder {
            ReferenceId = faker.Random.AlphaNumeric(10),
            DatabaseId = faker.Random.Int(1, 999),
            Status = faker.Random.AlphaNumeric(3),
            BlobUrl = faker.Internet.Url()
        };

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(
            $"\nReceived IGetTransportOrderStatusRequest request {JsonSerializer.Serialize(context.Message)}");


        var ids = await _repository.GetAllIdsAsync(CancellationToken.None);

        await context.RespondAsync<ITransportOrderStatusResponse>(response);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nResponded with {JsonSerializer.Serialize(response)}");
    }
}
