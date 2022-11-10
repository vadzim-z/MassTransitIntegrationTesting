using ActiveMQ_PoC.WebApp.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveMQ_PoC.IntegrationTests.TestFramework;

public class ApiWebApplicationFactory
{
    public WebApplicationFactory<Program> WebApplicationFactory
        => new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b => b.ConfigureServices(services
                => RegisterServices((ServiceCollection)services)));

    private void RegisterServices(ServiceCollection services)
        => services.AddMassTransitTestHarness(cfg => 
            cfg.AddConsumer<GetStatusConsumer>());
}