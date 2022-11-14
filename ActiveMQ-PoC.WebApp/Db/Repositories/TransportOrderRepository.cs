using ActiveMQ_PoC.Shared.Entities;
using ActiveMQ_PoC.Shared.Interfaces.Repositories;
using Finance.Common.Database.Relational.Interfaces;
using Finance.Common.Database.Relational.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ActiveMQ_PoC.WebApp.Db.Repositories;

public class TransportOrderRepository : AsyncRepository<TransportOrder, TransportOrderContext, int>, ITransportOrderRepository
{
    private readonly IUnitOfWork<TransportOrderContext> _uow;

    public TransportOrderRepository(IUnitOfWork<TransportOrderContext> uow) : base(uow) 
        => _uow = uow;

    public async Task<int> UpsertAsync(TransportOrder transportOrder, CancellationToken cancellationToken)
    {
        int id;
        var selectedTransportOrder = await _uow.DbContext.TransportOrders
            .FirstOrDefaultAsync(x => x.ReferenceId == transportOrder.ReferenceId,
                cancellationToken);

        id = selectedTransportOrder == null
            ? (await _uow.DbContext.TransportOrders.AddAsync(transportOrder, cancellationToken)).Entity.Id
            : _uow.DbContext.TransportOrders.Update(transportOrder).Entity.Id;
        //if (selectedTransportOrder == null)
        //{
        //    var entity = await _uow.DbContext.TransportOrders.AddAsync(transportOrder, cancellationToken);
        //    //await _uow.DbContext.SaveChangesAsync(cancellationToken);
        //    id = entity.Entity.Id;
        //}
        //else
        //{
        //    _uow.DbContext.TransportOrders.Update(transportOrder);
        //    //await _uow.DbContext.SaveChangesAsync(cancellationToken);
        //    id = transportOrder.Id;
        //}
        await _uow.DbContext.SaveChangesAsync(cancellationToken);

        return id;
    }

    public async Task<IEnumerable<TransportOrder>> GetAllAsync(CancellationToken cancellationToken)
        => await _uow.DbContext.TransportOrders.ToListAsync(cancellationToken);
}
