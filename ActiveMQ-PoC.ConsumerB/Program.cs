using ActiveMQ_PoC.ConsumerB;
using ActiveMQ_PoC.ConsumerB.Consumers;
using ActiveMQ_PoC.Shared.Constants;
using MassTransit;
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
                    x.AddConsumer<AuditEventConsumer>();
                    x.AddConsumer<TransportOrderAmendedEventConsumer>();
                    x.AddConsumer<CreateTransportOrderCommandConsumer>();

                    x.UsingActiveMq((context, cfg) =>
                    {
                        cfg.Host("localhost", 61616, h =>
                        {  
                            h.Username("admin");
                            h.Password("admin");
                        });

                        cfg.ReceiveEndpoint(QueueName.ConsumerB, e =>
                        {
                            e.ConfigureConsumer<AuditEventConsumer>(context);
                            e.ConfigureConsumer<TransportOrderAmendedEventConsumer>(context);
                            e.ConfigureConsumer<CreateTransportOrderCommandConsumer>(context);
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