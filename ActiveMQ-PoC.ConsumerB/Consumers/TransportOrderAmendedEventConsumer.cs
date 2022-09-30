using System.Text.Json;
using ActiveMQ_PoC.Shared.Interfaces;
using ActiveMQ_PoC.Shared.Interfaces.Events;
using MassTransit;

namespace ActiveMQ_PoC.ConsumerB.Consumers
{
    public class TransportOrderAmendedEventConsumer : IConsumer<ITransportOrderAmendedEvent>
    {
        public Task Consume(ConsumeContext<ITransportOrderAmendedEvent> context)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(
                $"\nReceived TransportOrderAmendedEvent : {JsonSerializer.Serialize(context.Message)} with ReferenceId : {context.Message.ReferenceId}");

            return Task.CompletedTask;
        }
    }
}
