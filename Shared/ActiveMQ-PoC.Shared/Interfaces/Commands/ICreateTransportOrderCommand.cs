using ActiveMQ_PoC.Shared.Interfaces.Events;

namespace ActiveMQ_PoC.Shared.Interfaces.Commands
{
    public interface ICreateTransportOrderCommand : IAuditEvent
    {
        public string TransportOrderNumber { get; }
        public string BlobUrl { get; }
    }
}
