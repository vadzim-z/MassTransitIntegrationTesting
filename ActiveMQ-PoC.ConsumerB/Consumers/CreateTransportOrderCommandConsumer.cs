using System.Text.Json;
using ActiveMQ_PoC.Shared.Interfaces.Commands;
using MassTransit;

namespace ActiveMQ_PoC.ConsumerB.Consumers;

public class CreateTransportOrderCommandConsumer : IConsumer<ICreateTransportOrderCommand>
{
    public Task Consume(ConsumeContext<ICreateTransportOrderCommand> context)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(
            $"\nReceived Create Command : {JsonSerializer.Serialize(context.Message)} with ReferenceId : {context.Message.ReferenceId}");

        return Task.CompletedTask;
    }
}