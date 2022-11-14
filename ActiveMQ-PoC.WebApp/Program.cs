using ActiveMQ_PoC.Shared.Interfaces.Repositories;
using ActiveMQ_PoC.WebApp.Consumers;
using ActiveMQ_PoC.WebApp.Db;
using ActiveMQ_PoC.WebApp.Db.Repositories;
using Finance.Common.Database.Relational.Extensions;
using MassTransit;
using SqlKata.Compilers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScopedPersistence<TransportOrderContext>();
builder.Services.AddQueryExecution<TransportOrderContext, PostgresCompiler>();
builder.Services.AddScoped<ITransportOrderRepository, TransportOrderRepository>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<GetStatusConsumer>();

    x.UsingActiveMq((context, cfg) =>
    {
        cfg.Host("localhost", 61616, h =>
        {
            h.Username("admin");
            h.Password("admin");
        });
        cfg.ConfigureEndpoints(context);
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }