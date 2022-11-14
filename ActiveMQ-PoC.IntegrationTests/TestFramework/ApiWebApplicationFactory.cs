using ActiveMQ_PoC.Shared.Interfaces.Repositories;
using ActiveMQ_PoC.WebApp.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ActiveMQ_PoC.IntegrationTests.TestFramework;

public class ApiWebApplicationFactory
{
    public Mock<ITransportOrderRepository> MockTransportOrderRepository { get; private set; }

    public ApiWebApplicationFactory() => MockTransportOrderRepository = new Mock<ITransportOrderRepository>();

    public WebApplicationFactory<Program> WebApplicationFactory
        => new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b => b.ConfigureServices(services
                => RegisterServices((ServiceCollection)services)));

    private void RegisterServices(ServiceCollection services)
        => services.AddMassTransitTestHarness(cfg =>
        {
            cfg.AddConsumer<GetStatusConsumer>();
            cfg.AddConsumer<UpsertTransportOrderConsumer>();
            //cfg.AddSingleton(MockTransportOrderRepository.Object);
        });
}