using ActiveMQ_PoC.Shared.Entities;
using ActiveMQ_PoC.WebApp.Db;
using FizzWare.NBuilder;

namespace ActiveMQ_PoC.IntegrationTests.TestFramework.Helpers;
internal class Utilities
{
    public static void InitializeDbForTests(TransportOrderContext db)
    {
        db.TransportOrders.AddRange(GetSeedingTransportOrders());
        db.SaveChanges();
    }

    public static void ReinitializeDbForTests(TransportOrderContext db)
    {
        db.TransportOrders.RemoveRange(db.TransportOrders);
        InitializeDbForTests(db);
    }

    public static List<TransportOrder> GetSeedingTransportOrders()
    {
        var builder = new Builder();

        return builder.CreateListOfSize<TransportOrder>(5)
            .Build()
            .ToList();
    }
}
