using System.Text.Json;
using ActiveMQ_PoC.Shared.Interfaces.Events;
using MassTransit;

namespace ActiveMQ_PoC.ConsumerB.Consumers
{
    public class AuditEventConsumer : IConsumer<IAuditEvent>
    {
        public Task Consume(ConsumeContext<IAuditEvent> context)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(
                $"\nReceived IAuditEvent : {JsonSerializer.Serialize(context.Message)}");

            return Task.CompletedTask;
        }
    }
}
