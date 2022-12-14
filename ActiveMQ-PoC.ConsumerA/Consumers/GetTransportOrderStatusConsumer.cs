using System.Text.Json;
using ActiveMQ_PoC.Shared.Interfaces.Requests;
using Bogus;
using MassTransit;

namespace ActiveMQ_PoC.ConsumerA.Consumers;

public class GetTransportOrderStatusConsumer : IConsumer<IGetTransportOrderStatusRequest>
{
    public async Task Consume(ConsumeContext<IGetTransportOrderStatusRequest> context)
    {
        var faker = new Faker();
        var response = new { ReferenceId = faker.Random.AlphaNumeric(10), DatabaseId = faker.Random.Int(1, 999), Status = faker.Random.AlphaNumeric(3), BlobUrl = faker.Internet.Url() };

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(
            $"\nReceived IGetTransportOrderStatusRequest request {JsonSerializer.Serialize(context.Message)}");

        await context.RespondAsync<ITransportOrderResponse>(response);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nResponded with {JsonSerializer.Serialize(response)}");
    }
}