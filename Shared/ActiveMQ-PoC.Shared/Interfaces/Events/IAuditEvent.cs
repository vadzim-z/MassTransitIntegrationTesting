namespace ActiveMQ_PoC.Shared.Interfaces.Events
{
    public interface IAuditEvent
    {
        public int DatabaseId { get; }
        public string ReferenceId { get; }
        public DateTime EventDateTime { get; }
    }
}
