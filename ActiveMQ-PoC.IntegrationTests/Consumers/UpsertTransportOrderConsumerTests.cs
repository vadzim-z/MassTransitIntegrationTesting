using ActiveMQ_PoC.IntegrationTests.TestFramework;
using ActiveMQ_PoC.Shared.Interfaces.Repositories;
using ActiveMQ_PoC.WebApp.Consumers;
using MassTransit;
using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ActiveMQ_PoC.Shared.Interfaces.Requests;
using Microsoft.Extensions.DependencyInjection;
using MassTransit.Testing;

namespace ActiveMQ_PoC.IntegrationTests.Consumers
{
    public class UpsertTransportOrderConsumerTests
    {
        private readonly ApiWebApplicationFactory _fixture;
        private readonly Mock<ITransportOrderRepository> _mockTransportOrderRepository;

        public UpsertTransportOrderConsumerTests(ApiWebApplicationFactory fixture)
        {
            _fixture = fixture;
            _mockTransportOrderRepository = fixture.MockTransportOrderRepository;
        }

        [Fact]
        public async Task Consume_UpsertTransportOrder_DataInserted()
        {
            // Arrange
            var webApplicationFactory = _fixture.WebApplicationFactory;
            var testHarness = await StartTestHarness(webApplicationFactory.Services);
            var transportOrderStatusRequest = new GetStatusRequest(NewId.NextGuid().ToString());

            // Act
            //SendGetTransportOrderStatusMessage(transportOrderStatusRequest, webApplicationFactory.Services);

            //// Assert
            //await AssertMessageConsumedAsync(webApplicationFactory.Services, transportOrderStatusRequest, testHarness);
        }

        private static void SendUpsertTransportOrderMessage(IUpsertTransportOrderRequest upsertTransportOrderRequest, IServiceProvider provider)
        {
            var requestClient = provider.GetRequiredService<IBus>().CreateRequestClient<IUpsertTransportOrderRequest>();
            requestClient.GetResponse<IUpsertTransportOrderRequest>(upsertTransportOrderRequest);
        }

        private static async Task<ITestHarness> StartTestHarness(IServiceProvider provider)
        {
            var testHarness = provider.GetRequiredService<ITestHarness>();
            await testHarness.Start();

            return testHarness;
        }
    }
}
