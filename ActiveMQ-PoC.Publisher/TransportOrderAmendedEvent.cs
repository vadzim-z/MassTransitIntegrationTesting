using ActiveMQ_PoC.Shared.Interfaces;
using ActiveMQ_PoC.Shared.Interfaces.Events;

namespace ActiveMQ_PoC.Publisher
{
    public class TransportOrderAmendedEvent : ITransportOrderAmendedEvent
    {
        public int DatabaseId { get; }
        public string ReferenceId { get; }
        public DateTime EventDateTime { get; }
        public string BlobUrl { get; }

        public TransportOrderAmendedEvent(int databaseId, string referenceId, DateTime eventDateTime, string blobUrl)
        {
            DatabaseId = databaseId;
            ReferenceId = referenceId;
            EventDateTime = eventDateTime;
            BlobUrl = blobUrl;
        }
    }
}
