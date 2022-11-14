using ActiveMQ_PoC.Shared.Entities;

namespace ActiveMQ_PoC.Shared.Interfaces.Repositories;
public interface ITransportOrderRepository
{
    Task<int> UpsertAsync(TransportOrder transportOrder, CancellationToken cancellationToken);
    Task<IEnumerable<TransportOrder>> GetAllAsync(CancellationToken cancellationToken);
}
