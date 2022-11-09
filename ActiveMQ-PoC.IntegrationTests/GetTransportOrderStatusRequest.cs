using ActiveMQ_PoC.Shared.Interfaces.Requests;

namespace ActiveMQ_PoC.IntegrationTests;
internal class GetTransportOrderStatusRequest : IGetTransportOrderStatusRequest
{
    public string ReferenceId { get; }

    public GetTransportOrderStatusRequest(string referenceId)
    {
        ReferenceId = referenceId;
    }
}
