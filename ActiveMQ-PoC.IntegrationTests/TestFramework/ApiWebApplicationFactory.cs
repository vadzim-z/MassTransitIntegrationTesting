using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace ActiveMQ_PoC.IntegrationTests.TestFramework;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config => { });
        builder.ConfigureTestServices(services => { });
        builder.ConfigureServices(svcs => svcs.AddMassTransitTestHarness());
    }
}