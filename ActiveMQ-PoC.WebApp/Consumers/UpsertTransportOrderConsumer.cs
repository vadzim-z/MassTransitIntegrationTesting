using ActiveMQ_PoC.Shared.Interfaces.Requests;
using MassTransit;
using MassTransit.ActiveMqTransport.Topology;

namespace ActiveMQ_PoC.WebApp.Consumers;

public class UpsertTransportOrderConsumer : IConsumer<IUpsertTransportOrderRequest>
{
}
