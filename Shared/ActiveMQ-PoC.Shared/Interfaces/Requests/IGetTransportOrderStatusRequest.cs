namespace ActiveMQ_PoC.Shared.Interfaces.Requests;

public interface IGetTransportOrderStatusRequest
{
    public string ReferenceId { get; }
}