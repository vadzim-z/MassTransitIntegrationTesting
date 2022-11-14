using ActiveMQ_PoC.Shared.Interfaces.Requests;
using Bogus;
using MassTransit;
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
        var transportOrders = await _repository.GetAllAsync(CancellationToken.None);
        var last = transportOrders.Last();
        var response = new TransportOrder
        {
            ReferenceId = last.ReferenceId,
            DatabaseId = last.DatabaseId,
            Status = last.Status,
            BlobUrl = last.BlobUrl
        };

        await context.RespondAsync<ITransportOrderResponse>(response);
    }
}
