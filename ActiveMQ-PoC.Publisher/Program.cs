using System.Text.Json;
using ActiveMQ_PoC.Shared.Interfaces;
using ActiveMQ_PoC.Shared.Interfaces.Events;
using ActiveMQ_PoC.Shared.Interfaces.Requests;
using Bogus;
using MassTransit;

namespace ActiveMQ_PoC.Publisher;

public class Program
{
    static IBusControl _busControl;
    public static async Task Main(string[] args)
    {
        _busControl = Bus.Factory.CreateUsingActiveMq(cfg =>
        {
            cfg.Host("localhost", 61616, cfgHost =>
            {
                cfgHost.Username("admin");
                cfgHost.Password("admin");
            });
        });

        await _busControl.StartAsync();

        Console.Write("\nPress 'Escape' to exit the process...");
        ConsoleKeyInfo cki;

        do
        {
            cki = Console.ReadKey();
            if (cki.Key != ConsoleKey.R)
            {
                await SendMessage();
            }
            else
            {
                await SendRequest();
            }
        } while (cki.Key != ConsoleKey.Escape);

        await _busControl.StopAsync();
    }

    private static async Task SendMessage()
    {
        var faker = new Faker();
        TransportOrderAmendedEvent e = new TransportOrderAmendedEvent(faker.Random.Int(1, 999), faker.Random.AlphaNumeric(10),
            faker.Date.Future(), faker.Internet.Url());
        await _busControl.Publish<ITransportOrderAmendedEvent>(e);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nSent: {JsonSerializer.Serialize(e)}");
    }

    private static async Task SendRequest()
    {
        var request = new {ReferenceId = new Faker().Random.AlphaNumeric(10)};
        var client = _busControl.CreateRequestClient<IGetTransportOrderStatusRequest>();
        var response = await client.GetResponse<ITransportOrderStatusResponse>(request);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nRequested with : {JsonSerializer.Serialize(request)}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nReceived Response : {JsonSerializer.Serialize(response)}");
    }
}