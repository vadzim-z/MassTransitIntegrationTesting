using ActiveMQ_PoC.ConsumerA.Consumers;
using ActiveMQ_PoC.Shared.Constants;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<TransportOrderAmendedEventConsumer>();
                    x.AddConsumer<GetTransportOrderStatusConsumer>();

                    x.UsingActiveMq((context, cfg) =>
                    {
                        cfg.Host("localhost", 61616, h =>
                        {  
                            h.Username("admin");
                            h.Password("admin");
                        });

                        cfg.ReceiveEndpoint(QueueName.ConsumerA, e =>
                        {
                            e.ConfigureConsumer<TransportOrderAmendedEventConsumer>(context);
                            e.ConfigureConsumer<GetTransportOrderStatusConsumer>(context);
                        });
                    });
                });
            })
            .Build()
            .RunAsync();

        Console.Write("\nPress 'Enter' to exit the process...");
  
        // another use of "Console.ReadKey()" method
        // here it asks to press the enter key to exit
        while (Console.ReadKey().Key != ConsoleKey.Enter) {

        }
    }
}