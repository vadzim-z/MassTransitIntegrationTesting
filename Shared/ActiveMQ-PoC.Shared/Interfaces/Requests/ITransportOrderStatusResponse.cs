namespace ActiveMQ_PoC.Shared.Interfaces.Requests;

public interface ITransportOrderStatusResponse
{
    public string ReferenceId { get; }
    public int DatabaseId { get; }
    public string Status { get; }
    public string BlobUrl { get; }
}