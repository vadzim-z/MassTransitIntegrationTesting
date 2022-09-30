namespace ActiveMQ_PoC.Shared.Interfaces.Events
{
    public interface ITransportOrderAmendedEvent : IAuditEvent
    {
        public string BlobUrl { get; }
    }
}
