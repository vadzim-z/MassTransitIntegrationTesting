using ActiveMQ_PoC.Shared.Interfaces.Repositories;
using ActiveMQ_PoC.Shared.Interfaces.Requests;
using MassTransit;
using MassTransit.ActiveMqTransport.Topology;

namespace ActiveMQ_PoC.WebApp.Consumers;

public class UpsertTransportOrderConsumer : IConsumer<IUpsertTransportOrderRequest>
{
    private readonly ITransportOrderRepository _repository;

    public UpsertTransportOrderConsumer(ITransportOrderRepository repository)
    {
        _repository = repository;
    }

    public Task Consume(ConsumeContext<IUpsertTransportOrderRequest> context)
    {
        var msg = context.Message;
        
        return Task.CompletedTask;
    }
}
