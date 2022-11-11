using Finance.Common.Database.Relational.Interfaces.Entities;

namespace ActiveMQ_PoC.Shared.Entities;

public class TransportOrder : IEntity<int>
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public int DatabaseId { get; set; }
    public string Status { get; set; }
    public string BlobUrl { get; set; }
}