using ActiveMQ_PoC.Shared.Interfaces.Requests;

namespace ActiveMQ_PoC.IntegrationTests;
internal class GetStatusRequest : IGetTransportOrderStatusRequest
{
    public string ReferenceId { get; }

    public GetStatusRequest(string referenceId)
    {
        ReferenceId = referenceId;
    }
}
