using ActiveMQ_PoC.Shared.Entities;
using Finance.Common.Database.Relational.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ActiveMQ_PoC.WebApp.Db;

public class TransportOrderContext : NpgsqlContextBase
{
    private readonly string _connectionString;
    private const string ConnectionStringKey = "DB_CONNECTION_STRING";

    public DbSet<TransportOrder> TransportOrders { get; set; }

    public TransportOrderContext(DbContextOptions options, IConfiguration configuration)
        : base(options)
    {
        _connectionString = configuration.GetConnectionString(ConnectionStringKey) ??
                            "User Id = postgres; Password=postgres;Server=localhost;Port=5432;Database=ActiveMQ-poc;Integrated Security = true; Pooling=true";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TransportOrder>()
            .ToTable(nameof(TransportOrders));
    }
}
